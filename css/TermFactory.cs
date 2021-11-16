using System;
using System.Collections.Generic;

namespace StyleParserCS.css
{

    public interface TermFactory
    {

        TermAngle createAngle(float value);
        TermAngle createAngle(string value, TermLength_Unit unit, int unary);

        //ORIGINAL LINE: TermCalc createCalc(java.util.List<Term<>> args);
        TermCalc createCalc(IList<Term> args);

        TermColor createColor(TermIdent ident);
        TermColor createColor(string hash);
        TermColor createColor(int r, int g, int b);
        TermColor createColor(int r, int g, int b, int a);
        TermColor createColor(TermFunction function);

        TermFrequency createFrequency(float value);
        TermFrequency createFrequency(string value, TermLength_Unit unit, int unary);

        TermExpression createExpression(string expr);

        TermFunction createFunction(string name);
        //ORIGINAL LINE: TermFunction createFunction(String name, java.util.List<Term<>> args);
        TermFunction createFunction(string name, IList<Term> args);

        TermIdent createIdent(string value);
        TermIdent createIdent(string value, bool dash);
        TermBracketedIdents createBracketedIdents();
        TermBracketedIdents createBracketedIdents(int initialSize);

        TermInteger createInteger(int value);
        TermInteger createInteger(string value, int unary);

        TermLength createLength(float value);
        TermLength createLength(float value, TermLength_Unit unit);
        TermLength createLength(string value, TermLength_Unit unit, int unary);

        TermList createList();
        TermList createList(int initialSize);

        TermNumber createNumber(float value);
        TermNumber createNumber(string value, int unary);

        //ORIGINAL LINE: TermNumeric<> createNumeric(String value, int unary);
        TermNumeric createNumeric(string value, int unary);

        TermNumeric<float> createDimension(string value, int unary);

        TermPair<K, V> createPair<K, V>(K key, V value);

        TermPercent createPercent(float value);
        TermPercent createPercent(string value, int unary);

        //ORIGINAL LINE: TermPropertyValue createPropertyValue(CSSProperty property, Term<> value);
        TermPropertyValue createPropertyValue(CSSProperty property, Term value);

        TermRect createRect(TermFunction function);
        /// <summary>
        /// Creates a rectangle from four lengths. Use {@code null} for {@code auto} values.
        /// </summary>
        TermRect createRect(TermLength a, TermLength b, TermLength c, TermLength d);

        TermResolution createResolution(float value);
        TermResolution createResolution(string value, TermLength_Unit unit, int unary);

        TermString createString(string value);

        Term<V> createTerm<V>(V value);

        TermTime createTime(float value);
        TermTime createTime(float value, TermLength_Unit unit);
        TermTime createTime(string value, TermLength_Unit unit, int unary);

        TermUnicodeRange createUnicodeRange(string value);

        TermURI createURI(string value);
        TermURI createURI(string value, Uri basev);

        TermOperator createOperator(char value);
    }

}