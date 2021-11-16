using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;

namespace StyleParserCS.csskit.antlr4
{
    using TypeMapper = StyleParserCS.csskit.antlr4.CSSToken.TypeMapper;

    public class CSSTokenFactory
    {

        private readonly Tuple<ITokenSource, ICharStream> input;
        private readonly Lexer lexer;
        private readonly CSSLexerState ls;
        private readonly TypeMapper typeMapper;
        private readonly ITokenFactory factory;


        public CSSTokenFactory(Tuple<ITokenSource, ICharStream> input, Lexer lexer, CSSLexerState ls, Type lexerClass)
        {
            this.input = input;
            this.lexer = lexer;
            this.ls = ls;
            this.typeMapper = CSSToken.createDefaultTypeMapper(lexerClass);
        }

        public CSSTokenFactory(ITokenFactory factory, Lexer lexer, CSSLexerState ls, Type lexerClass)
        {
            this.input = null;
            this.factory = factory;
            this.lexer = lexer;
            this.ls = ls;
            this.typeMapper = CSSToken.createDefaultTypeMapper(lexerClass);
        }

        public virtual CSSToken make()
        {
            // CSSToken t1 = this.factory.Create()
            CSSToken t = new CSSToken(input, lexer.Type, lexer.Channel, lexer.TokenStartCharIndex, input.Item2.Index - 1, typeMapper);
            t.Line = lexer.TokenStartLine;
            t.Text = lexer.Text;
            t.CharPositionInLine = lexer.TokenStartCharIndex;
            t.Base = ((CSSInputStream)input.Item2).Base;

            // clone lexer state
            t.setLexerState(new CSSLexerState(ls));
            return t;
        }
    }

}