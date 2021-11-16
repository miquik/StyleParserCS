using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StyleParserCS.csskit.antlr4
{
    public class CSSTokenRecovery
    {

        private readonly Lexer lexer;
        private readonly ICharStream input;
        //    private final Recognizer state;
        private readonly CSSLexerState ls;
        // private readonly Logger log;
        private readonly Stack<int> expectedToken;

        private bool eof;

        // tokens
        public const int APOS = 1;
        public const int QUOT = 2;
        public const int RPAREN = 3;
        public const int RCURLY = 4;
        public const int IMPORT = 5;
        public const int CHARSET = 6;
        public const int STRING = 7;
        public const int INVALID_STRING = 8;
        public const int RBRACKET = 9;

        private readonly CSSToken.TypeMapper typeMapper;
        private readonly CSSToken.TypeMapper lexerTypeMapper;

        private FieldInfo _tokenStartCharIndex;
        private FieldInfo _tokenStartLine;
        private FieldInfo _tokenStartColumn;
        private FieldInfo _charPositionInLine;

        public CSSTokenRecovery(Lexer lexer, ICharStream input, CSSLexerState ls)
        {
            this.lexer = lexer;
            this.input = input;
            //        this.state = state;
            this.ls = ls;
            this.expectedToken = new Stack<int>();
            this.eof = false;
            lexerTypeMapper = CSSToken.createDefaultTypeMapper(lexer.GetType());
            typeMapper = new CSSToken.TypeMapper(typeof(CSSTokenRecovery), lexer.GetType(), "APOS", "QUOT", "RPAREN", "RCURLY", "IMPORT", "CHARSET", "STRING", "INVALID_STRING", "RBRACKET");

            // reflection workaraound
            // Antlr4 4.9.2 - CSharp don't expose public fields
            // private int _tokenStartCharIndex = -1;
            // private int _tokenStartLine;
            // private int _tokenStartColumn;
            Type lexerType = lexer.GetType();
            _tokenStartCharIndex = lexerType.BaseType.GetField("_tokenStartCharIndex", BindingFlags.Instance | BindingFlags.NonPublic);
            _tokenStartLine = lexerType.BaseType.GetField("_tokenStartLine", BindingFlags.Instance | BindingFlags.NonPublic);
            _tokenStartColumn = lexerType.BaseType.GetField("_tokenStartColumn", BindingFlags.Instance | BindingFlags.NonPublic);
            // ((LexerATNSimulator)lexer.Interpreter).CharPositionInLine
            _charPositionInLine = lexer.Interpreter.GetType().GetField("charPositionInLine", BindingFlags.Instance | BindingFlags.NonPublic);
            int kk = 0;
        }

        public virtual bool AtEof
        {
            get
            {
                return eof;
            }
        }

        public virtual void expecting(int token)
        {
            expectedToken.Push(token);
        }

        public virtual void end()
        {
            expectedToken.Pop();
        }

        public virtual bool recover()
        {
            // there is no special recovery
            if (expectedToken.Count == 0)
            {
                return false;
            }
            int? t = typeMapper.inverse().get(expectedToken.Pop()); 
            if (t == null)
            {
                return false;
            }
            /*
            try
            {
                t = typeMapper.inverse().get(expectedToken.Pop());
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            */
            switch (t.Value)
            {
                case IMPORT: // IMPORT share recovery rules with CHARSET
                case CHARSET:
                    //ORIGINAL LINE: final org.antlr.v4.runtime.misc.IntervalSet charsetFollow = new org.antlr.v4.runtime.misc.IntervalSet((int) '}', (int) ';');
                    IntervalSet charsetFollow = new IntervalSet((int)'}', (int)';');
                    consumeUntilBalanced(charsetFollow);
                    break;
                case STRING:
                    // eat character which should be newline but not EOF
                    if (consumeAnyButEOF())
                    {
                        // set back to uninitialized state
                        ls.quotOpen = false;
                        ls.aposOpen = false;
                        // create invalid string token
                        lexer.Token = (IToken)new CSSToken(typeMapper.get(INVALID_STRING).Value, ls, lexerTypeMapper);
                        ((CSSToken)lexer.Token).Text = "INVALID_STRING";
                        //                    state.token = (Token) new CSSToken(typeMapper.get(INVALID_STRING), ls, lexer.getClass());
                        //                    state.token.setText("INVALID_STRING");
                    }
                    // we can't just let parser generate missing
                    // single/double quot token
                    // because we have not emitted content of string yet!
                    // we will fake string token
                    else
                    {
                        char enclosing = (ls.quotOpen) ? '"' : '\'';
                        ls.quotOpen = false;
                        ls.aposOpen = false;
                        lexer.Token = (IToken)new CSSToken(typeMapper.get(STRING).Value, ls, lexer.TokenStartCharIndex, input.Index - 1, lexerTypeMapper);
                        ((CSSToken)lexer.Token).Text = input.ToString().Substring(lexer.TokenStartCharIndex, (input.Index - 1) - lexer.TokenStartCharIndex) + enclosing;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Implements Lexer's next token with extra token passing from
        /// recovery function
        /// </summary>
        // TOCHECK!!!!!!
        public virtual IToken nextToken()
        {
            if (lexer.InputStream == null)
            {
                throw new System.InvalidOperationException("nextToken requires a non-null input stream.");
            }
            else
            {
                int tokenStartMarker = lexer.InputStream.Mark();

                try
                {
                    IToken ttype1;
                    while (!lexer.HitEOF)
                    {
                        lexer.Token = null;
                        lexer.Channel = TokenConstants.DefaultChannel;
                        _tokenStartCharIndex.SetValue(lexer, lexer.InputStream.Index);
                        _tokenStartColumn.SetValue(lexer, _charPositionInLine.GetValue((LexerATNSimulator)lexer.Interpreter)); // .CharPositionInLine);
                        _tokenStartLine.SetValue(lexer, ((LexerATNSimulator)lexer.Interpreter).Line);
                        /*
                        lexer.Channel = TokenConstants.DefaultChannel;
                        lexer.TokenStartCharIndex = lexer.InputStream.Index;
                        lexer.TokenStartCharPositionInLine = ((LexerATNSimulator)lexer.Interpreter).CharPositionInLine;
                        lexer.TokenStartLine = ((LexerATNSimulator)lexer.Interpreter).Line;
                        */
                        lexer.Text = null;


                        do
                        {
                            lexer.Type = 0;

                            int ttype;
                            try
                            {
                                ttype = ((LexerATNSimulator)lexer.Interpreter).Match((ICharStream)lexer.InputStream, lexer.CurrentMode);
                            }
                            catch (LexerNoViableAltException var7)
                            {
                                lexer.NotifyListeners(var7);
                                lexer.Recover(var7);
                                ttype = -3;
                            }

                            if (lexer.InputStream.LA(1) == -1)
                            {
                                lexer.HitEOF = true;
                            }

                            if (lexer.Type == 0)
                            {
                                lexer.Type = ttype;
                            }

                            if (lexer.Type == -3)
                            {
                                goto label110Continue;
                            }
                        } while (lexer.Type == -2);

                        if (lexer.Token == null)
                        {
                            lexer.Emit();
                        }

                        ttype1 = lexer.Token;
                        //log.trace("return token: >" + ttype1.getText()+"< " + ttype1.getType());
                        return ttype1;
                    label110Continue:;
                    }
                label110Break:
                    // recover from unexpected EOF
                    eof = true;
                    if (!ls.Balanced)
                    {
                        return generateEOFRecover();
                    }
                    // log.trace("lexer state is balanced - emitEOF");
                    lexer.EmitEOF();
                    ttype1 = lexer.Token;
                    return ttype1;
                }
                finally
                {
                    lexer.InputStream.Release(tokenStartMarker);
                }
            }
        }


        /// <summary>
        /// Recovers from unexpected EOF by preparing
        /// new token
        /// </summary>
        public virtual CSSToken generateEOFRecover()
        {

            CSSToken t = null;

            if (ls.aposOpen)
            {
                ls.aposOpen = false;
                t = new CSSToken(typeMapper.get(APOS).Value, ls, lexerTypeMapper);
                t.Text = "'";
            }
            else if (ls.quotOpen)
            {
                ls.quotOpen = false;
                t = new CSSToken(typeMapper.get(QUOT).Value, ls, lexerTypeMapper);
                t.Text = "\"";
            }
            else if (ls.parenNest != 0)
            {
                ls.parenNest--;
                t = new CSSToken(typeMapper.get(RPAREN).Value, ls, lexerTypeMapper);
                t.Text = ")";
            }
            else if (ls.curlyNest != 0)
            {
                ls.curlyNest--;
                t = new CSSToken(typeMapper.get(RCURLY).Value, ls, lexerTypeMapper);
                t.Text = "}";
            }
            else if (ls.sqNest != 0)
            {
                ls.sqNest--;
                t = new CSSToken(typeMapper.get(RBRACKET).Value, ls, lexerTypeMapper);
                t.Text = "]";
            }

            // if (log.DebugEnabled)
            {
                // log.debug("Recovering from EOF by {}", t.Text);
            }
            return t;
        }

        /// <summary>
        /// Eats characters until on from follow is found and Lexer state
        /// is balanced at the moment
        /// </summary>
        private void consumeUntilBalanced(IntervalSet follow)
        {

            // if (log.DebugEnabled)
            {
                // log.debug("Lexer entered consumeUntilBalanced with {} and follow {}", ls, follow);
            }

            int c;
            do
            {
                c = input.LA(1);
                // change apostrophe state
                if (c == '\'' && ls.quotOpen == false)
                {
                    ls.aposOpen = !ls.aposOpen;
                }
                // change quot state
                else if (c == '"' && ls.aposOpen == false)
                {
                    ls.quotOpen = !ls.quotOpen;
                }
                else if (c == '(')
                {
                    ls.parenNest++;
                }
                else if (c == ')' && ls.parenNest > 0)
                {
                    ls.parenNest--;
                }
                else if (c == '{')
                {
                    ls.curlyNest++;
                }
                else if (c == '}' && ls.curlyNest > 0)
                {
                    ls.curlyNest--;
                }
                // handle end of line in string
                else if (c == '\n')
                {
                    if (ls.quotOpen)
                    {
                        ls.quotOpen = false;
                    }
                    else if (ls.aposOpen)
                    {
                        ls.aposOpen = false;
                    }
                }
                else if (c == TokenConstants.EOF)
                {
                    // log.info("Unexpected EOF during consumeUntilBalanced, EOF not consumed");
                    return;
                }

                input.Consume();
                // log result
                // if (log.TraceEnabled)
                {
                    // log.trace("Lexer consumes '{}'({}) until balanced ({}).", new object[] { Convert.ToString((char)c), Convert.ToString(c), ls });
                }

            } while (!(ls.Balanced && follow.Contains(c)));
        }

        /// <summary>
        /// Consumes arbitrary character but EOF
        /// </summary>
        /// <returns> <code>false</code> if EOF was matched,
        /// <code>true</code> otherwise and that character was consumed </returns>
        private bool consumeAnyButEOF()
        {

            int c = input.LA(1);

            if (c == IntStreamConstants.EOF)
            {
                return false;
            }

            //if (log.TraceEnabled)
            {
                //log.trace("Lexer consumes '{}' while consumeButEOF", Convert.ToString((char)c));
            }

            // consume character
            input.Consume();
            return true;
        }
    }

}