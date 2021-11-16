using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

namespace StyleParserCS.csskit.antlr4
{
    public class CSSErrorStrategy : DefaultErrorStrategy
    {
        // private Logger logger;

        public CSSErrorStrategy()
        {
            // this.logger = org.slf4j.LoggerFactory.getLogger(this.GetType());
            // // logger.trace("CssErrorStrategy instantiated");
        }


        //ORIGINAL LINE: public void sync(Parser recognizer) throws RecognitionException
        public virtual void sync(Parser recognizer)
        {
            ATNState s = recognizer.Interpreter.atn.states[recognizer.State];
            if (!this.InErrorRecoveryMode(recognizer))
            {
                ITokenStream tokens = (ITokenStream)recognizer.InputStream;
                int la = tokens.LA(1);
                if (!recognizer.Atn.NextTokens(s).Contains(la) && la != -1)
                {
                    if (!recognizer.IsExpectedToken(la))
                    {
                        switch ((int)s.StateType)
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 10:
                                throw new RecognitionException(recognizer, tokens, recognizer.Context);
                            case 9:
                            case 11:
                                //added
                                this.ReportUnwantedToken(recognizer);
                                throw new RecognitionException(recognizer, tokens, recognizer.Context);
                            case 6:
                            case 7:
                            case 8:
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// throws RecognitionException to handle in parser's catch block
        /// </summary>
        //ORIGINAL LINE: public IToken recoverInline(Parser recognizer) throws RecognitionException
        public virtual IToken recoverInline(Parser recognizer)
        {
            throw new RecognitionException(recognizer, recognizer.InputStream, recognizer.Context);
        }

        /// <summary>
        /// Consumes token until lexer state is balanced and
        /// token from follow is matched. Matched token is also consumed
        /// </summary>
        protected internal virtual void consumeUntilGreedy(Parser recognizer, IntervalSet follow)
        {
            // // logger.trace("CONSUME UNTIL GREEDY {}", follow.ToString());
            for (int ttype = recognizer.InputStream.LA(1); ttype != -1 && !follow.Contains(ttype); ttype = recognizer.InputStream.LA(1))
            {
                IToken ty = recognizer.Consume();
                // // logger.trace("Skipped greedy: {}", t.Text);
            }
            IToken t = recognizer.Consume();
            //// logger.trace("Skipped greedy: {} follow: {}", t.Text, follow);

        }

        /// <summary>
        /// Consumes token until lexer state is function-balanced and
        /// token from follow is matched. Matched token is also consumed
        /// </summary>
        protected internal virtual void consumeUntilGreedy(Parser recognizer, IntervalSet set, CSSLexerState.RecoveryMode mode)
        {
            CSSToken t;
            do
            {
                IToken next = ((ITokenStream)recognizer.InputStream).LT(1);
                if (next is CSSToken)
                {
                    t = (CSSToken)((ITokenStream)recognizer.InputStream).LT(1);
                    if (t.Type == TokenConstants.EOF)
                    {
                        // logger.trace("token eof ");
                        break;
                    }
                }
                else
                {
                    break; // not a CSSToken, probably EOF
                }
                // logger.trace("Skipped greedy: {}", t.Text);
                // consume token even if it will match
                recognizer.Consume();
            } while (!(t.LexerState.isBalanced(mode, null, t) && set.Contains(t.Type)));
        }

        /// <summary>
        /// Consumes token until lexer state is function-balanced and
        /// token from follow is matched. Matched token is also consumed.
        /// </summary>
        public virtual void consumeUntilGreedy(Parser recognizer, IntervalSet follow, CSSLexerState.RecoveryMode mode, CSSLexerState ls)
        {
            consumeUntil(recognizer, follow, mode, ls);
            recognizer.InputStream.Consume();
        }

        /// <summary>
        /// Consumes token until lexer state is function-balanced and
        /// token from follow is matched.
        /// </summary>
        public virtual void consumeUntil(Parser recognizer, IntervalSet follow, CSSLexerState.RecoveryMode mode, CSSLexerState ls)
        {
            CSSToken t;
            bool finish;
            ITokenStream input = (ITokenStream)recognizer.InputStream;
            do
            {
                IToken next = input.LT(1);
                if (next is CSSToken)
                {
                    t = (CSSToken)input.LT(1);
                    if (t.Type == TokenConstants.EOF)
                    {
                        // logger.trace("token eof ");
                        break;
                    }
                }
                else
                {
                    break; // not a CSSToken, probably EOF
                }
                // consume token if does not match
                finish = (t.LexerState.isBalanced(mode, ls, t) && follow.Contains(t.Type));
                if (!finish)
                {
                    // logger.trace("Skipped: {}", t);
                    input.Consume();
                }
            } while (!finish);
        }
    }

}