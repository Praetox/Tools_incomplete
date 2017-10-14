using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Lundin
{
    public class ExpressionParser
    {
        // Fields
        private Hashtable htbl;
        private int maxoplength;
        private Hashtable ops = new Hashtable(0x34);
        private int sb_init;
        private Hashtable spconst = new Hashtable(12);
        private Hashtable trees = new Hashtable(0x65);
        private Hashtable htDummy = new Hashtable();

        // Methods
        public ExpressionParser()
        {
            this.ops.Add("^", new Operator("^", 2, 3));
            this.ops.Add("+", new Operator("+", 2, 6));
            this.ops.Add("-", new Operator("-", 2, 6));
            this.ops.Add("/", new Operator("/", 2, 4));
            this.ops.Add("*", new Operator("*", 2, 4));
            this.ops.Add("cos", new Operator("cos", 1, 2));
            this.ops.Add("sin", new Operator("sin", 1, 2));
            this.ops.Add("exp", new Operator("exp", 1, 2));
            this.ops.Add("ln", new Operator("ln", 1, 2));
            this.ops.Add("tan", new Operator("tan", 1, 2));
            this.ops.Add("acos", new Operator("acos", 1, 2));
            this.ops.Add("asin", new Operator("asin", 1, 2));
            this.ops.Add("atan", new Operator("atan", 1, 2));
            this.ops.Add("cosh", new Operator("cosh", 1, 2));
            this.ops.Add("sinh", new Operator("sinh", 1, 2));
            this.ops.Add("tanh", new Operator("tanh", 1, 2));
            this.ops.Add("sqrt", new Operator("sqrt", 1, 2));
            this.ops.Add("cotan", new Operator("cotan", 1, 2));
            this.ops.Add("fpart", new Operator("fpart", 1, 2));
            this.ops.Add("acotan", new Operator("acotan", 1, 2));
            this.ops.Add("round", new Operator("round", 1, 2));
            this.ops.Add("ceil", new Operator("ceil", 1, 2));
            this.ops.Add("floor", new Operator("floor", 1, 2));
            this.ops.Add("fac", new Operator("fac", 1, 2));
            this.ops.Add("sfac", new Operator("sfac", 1, 2));
            this.ops.Add("abs", new Operator("abs", 1, 2));
            this.ops.Add("log", new Operator("log", 2, 5));
            this.ops.Add("%", new Operator("%", 2, 4));
            this.ops.Add(">", new Operator(">", 2, 7));
            this.ops.Add("<", new Operator("<", 2, 7));
            this.ops.Add("&&", new Operator("&&", 2, 10));
            this.ops.Add("==", new Operator("==", 2, 8));
            this.ops.Add("!=", new Operator("!=", 2, 8));
            this.ops.Add("||", new Operator("||", 2, 9));
            this.ops.Add("!", new Operator("!", 1, 1));
            this.ops.Add(">=", new Operator(">=", 2, 7));
            this.ops.Add("<=", new Operator("<=", 2, 7));
            this.spconst.Add("euler", 2.7182818284590451);
            this.spconst.Add("pi", 3.1415926535897931);
            this.spconst.Add("nan", (double)1.0 / (double)0.0);
            this.spconst.Add("infinity", (double)1.0 / (double)0.0);
            this.spconst.Add("true", 1.0);
            this.spconst.Add("false", 0.0);
            this.maxoplength = 6;
            this.sb_init = 50;
        }

        private string arg(string _operator, string exp, int index)
        {
            int num3 = -1;
            int length = exp.Length;
            string str = null;
            StringBuilder builder = new StringBuilder(this.sb_init);
            int num2 = index;
            int num = 0;
            if (_operator == null)
            {
                num3 = -1;
            }
            else
            {
                num3 = ((Operator)this.ops[_operator]).precedence();
            }
            while (num2 < length)
            {
                if (exp[num2] == '(')
                {
                    num = this.match(exp, num2);
                    builder.Append(exp.Substring(num2, (num + 1) - num2));
                    num2 = num + 1;
                }
                else
                {
                    str = this.getOp(exp, num2);
                    if (str != null)
                    {
                        if (((builder.Length != 0) && !this.isTwoArgOp(this.backTrack(builder.ToString()))) && (((Operator)this.ops[str]).precedence() >= num3))
                        {
                            return builder.ToString();
                        }
                        builder.Append(str);
                        num2 += str.Length;
                        continue;
                    }
                    builder.Append(exp[num2]);
                    num2++;
                }
            }
            return builder.ToString();
        }

        private string backTrack(string str)
        {
            int num = 0;
            int length = str.Length;
            string str2 = null;
            try
            {
                for (num = 0; num <= this.maxoplength; num++)
                {
                    if (((str2 = this.getOp(str, ((length - 1) - this.maxoplength) + num)) != null) && (((((length - this.maxoplength) - 1) + num) + str2.Length) == length))
                    {
                        return str2;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private double fac(double val)
        {
            if (!this.isInteger(val))
            {
                return double.NaN;
            }
            if (val < 0.0)
            {
                return double.NaN;
            }
            if (val <= 1.0)
            {
                return 1.0;
            }
            return (val * this.fac(val - 1.0));
        }

        private double fpart(double val)
        {
            if (val >= 0.0)
            {
                return (val - Math.Floor(val));
            }
            return (val - Math.Ceiling(val));
        }

        private string get(string key)
        {
            object obj2 = this.htbl[key];
            string str = null;
            if (obj2 == null)
            {
                throw new Exception("No value associated with " + key);
            }
            try
            {
                str = (string)obj2;
            }
            catch
            {
                throw new Exception("Wrong type value for " + key + " expected String");
            }
            return str;
        }

        private string getOp(string exp, int index)
        {
            int num = 0;
            int length = exp.Length;
            for (num = 0; num < this.maxoplength; num++)
            {
                if ((index >= 0) && (((index + this.maxoplength) - num) <= length))
                {
                    string str = exp.Substring(index, this.maxoplength - num);
                    if (this.isOperator(str))
                    {
                        return str;
                    }
                }
            }
            return null;
        }

        private bool isAllNumbers(string str)
        {
            int num = 0;
            int length = 0;
            bool flag = false;
            char c = str[0];
            switch (c)
            {
                case '-':
                case '+':
                    num = 1;
                    break;
            }
            length = str.Length;
            while (num < length)
            {
                c = str[num];
                if (!char.IsDigit(c) && ((c != '.') || flag))
                {
                    return false;
                }
                if (c == '.')
                {
                    flag = true;
                }
                num++;
            }
            return true;
        }

        private bool isAllowedSym(char s)
        {
            if ((((s != ')') && (s != '(')) && ((s != '.') && (s != '>'))) && (((s != '<') && (s != '&')) && (s != '=')))
            {
                return (s == '|');
            }
            return true;
        }

        private bool isAlpha(char ch)
        {
            return (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z')));
        }

        private bool isConstant(char ch)
        {
            return char.IsDigit(ch);
        }

        private bool isConstant(string exp)
        {
            try
            {
                if (double.IsNaN(double.Parse(exp)))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool isEven(int a)
        {
            return this.isInteger((double)(a / 2));
        }

        private bool isInteger(double a)
        {
            return ((a - ((int)a)) == 0.0);
        }

        private bool isOperator(string str)
        {
            return this.ops.ContainsKey(str);
        }

        private bool isTwoArgOp(string str)
        {
            if (str == null)
            {
                return false;
            }
            object obj2 = this.ops[str];
            if (obj2 == null)
            {
                return false;
            }
            return (((Operator)obj2).arguments() == 2);
        }

        private bool isVariable(string str)
        {
            int index = 0;
            int length = str.Length;
            if (this.isAllNumbers(str))
            {
                return false;
            }
            for (index = 0; index < length; index++)
            {
                if ((this.getOp(str, index) != null) || this.isAllowedSym(str[index]))
                {
                    return false;
                }
            }
            return true;
        }

        private int match(string exp, int index)
        {
            int length = exp.Length;
            int num2 = index;
            int num3 = 0;
            while (num2 < length)
            {
                if (exp[num2] == '(')
                {
                    num3++;
                }
                else if (exp[num2] == ')')
                {
                    num3--;
                }
                if (num3 == 0)
                {
                    return num2;
                }
                num2++;
            }
            return index;
        }

        private bool matchParant(string exp)
        {
            int num = 0;
            int num2 = 0;
            int length = exp.Length;
            for (num2 = 0; num2 < length; num2++)
            {
                if (exp[num2] == '(')
                {
                    num++;
                }
                else if (exp[num2] == ')')
                {
                    num--;
                }
            }
            return (num == 0);
        }

        private Node parse(string exp)
        {
            int num;
            string str2;
            string str3;
            Node node = null;
            string str = str2 = str3 = "";
            int num2 = num = 0;
            int length = exp.Length;
            if (length == 0)
            {
                throw new Exception("Wrong number of arguments to operator");
            }
            if ((exp[0] == '(') && ((num2 = this.match(exp, 0)) == (length - 1)))
            {
                return this.parse(exp.Substring(1, num2 - 1));
            }
            if (this.isVariable(exp))
            {
                return new Node(exp);
            }
            if (!this.isAllNumbers(exp))
            {
                while (num < length)
                {
                    str3 = this.getOp(exp, num);
                    if (str3 == null)
                    {
                        str = this.arg(null, exp, num);
                        str3 = this.getOp(exp, num + str.Length);
                        if (str3 == null)
                        {
                            throw new Exception("Missing operator");
                        }
                        if (this.isTwoArgOp(str3))
                        {
                            str2 = this.arg(str3, exp, (num + str.Length) + str3.Length);
                            if (str2.Equals(""))
                            {
                                throw new Exception("Wrong number of arguments to operator " + str3);
                            }
                            node = new Node(str3, this.parse(str), this.parse(str2));
                            num += (str.Length + str3.Length) + str2.Length;
                        }
                        else
                        {
                            if (str.Equals(""))
                            {
                                throw new Exception("Wrong number of arguments to operator " + str3);
                            }
                            node = new Node(str3, this.parse(str));
                            num += str.Length + str3.Length;
                        }
                    }
                    else if (this.isTwoArgOp(str3))
                    {
                        str = this.arg(str3, exp, num + str3.Length);
                        if (str.Equals(""))
                        {
                            throw new Exception("Wrong number of arguments to operator " + str3);
                        }
                        if (node == null)
                        {
                            if (!str3.Equals("+") && !str3.Equals("-"))
                            {
                                throw new Exception("Wrong number of arguments to operator " + str3);
                            }
                            node = new Node(0.0);
                        }
                        node = new Node(str3, node, this.parse(str));
                        num += str.Length + str3.Length;
                    }
                    else
                    {
                        str = this.arg(str3, exp, num + str3.Length);
                        if (str.Equals(""))
                        {
                            throw new Exception("Wrong number of arguments to operator " + str3);
                        }
                        node = new Node(str3, this.parse(str));
                        num += str.Length + str3.Length;
                    }
                }
                return node;
            }
            return new Node(double.Parse(exp));
        }

        public double Parse(string exp, Hashtable tbl)
        {
            double num2;
            double num = 0.0;
            if ((exp == null) || exp.Equals(""))
            {
                throw new Exception("First argument to method eval is null or empty string");
            }
            if (tbl == null)
            {
                return this.Parse(exp, new Hashtable());
            }
            this.htbl = tbl;
            string key = this.skipSpaces(exp.ToLower());
            this.sb_init = key.Length;
            try
            {
                if (this.trees.ContainsKey(key))
                {
                    num = this.toValue((Node)this.trees[key]);
                }
                else
                {
                    this.Syntax(key);
                    Node tree = this.parse(this.putMult(this.parseE(key)));
                    num = this.toValue(tree);
                    this.trees.Add(key, tree);
                }
                num2 = num;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return num2;
        }

        public Int32 Parse(string Exp)
        {
            return (int)Math.Round(Parse(Exp, htDummy));
        }

        private string parseE(string exp)
        {
            int num2;
            StringBuilder builder = new StringBuilder(exp);
            int num = num2 = 0;
            int length = exp.Length;
            while (num < length)
            {
                try
                {
                    if (((exp[num] == 'e') && char.IsDigit(exp[num - 1])) && (char.IsDigit(exp[num + 1]) || (((exp[num + 1] == '-') || (exp[num + 1] == '+')) && char.IsDigit(exp[num + 2]))))
                    {
                        builder[num + num2] = '*';
                        builder.Insert((num + num2) + 1, "10^");
                        num2 += 3;
                    }
                }
                catch
                {
                }
                num++;
            }
            return builder.ToString();
        }

        private string putMult(string exp)
        {
            int index = 0;
            int num2 = 0;
            string str = null;
            StringBuilder builder = new StringBuilder(exp);
            int length = exp.Length;
            while (index < length)
            {
                try
                {
                    if ((((str = this.getOp(exp, index)) != null) && !this.isTwoArgOp(str)) && this.isAlpha(exp[index - 1]))
                    {
                        builder.Insert(index + num2, '*');
                        num2++;
                    }
                    else if ((this.isAlpha(exp[index]) && this.isConstant(exp[index - 1])) && ((str == null) || !str.Equals("log")))
                    {
                        builder.Insert(index + num2, '*');
                        num2++;
                    }
                    else if ((exp[index] == '(') && this.isConstant(exp[index - 1]))
                    {
                        builder.Insert(index + num2, '*');
                        num2++;
                    }
                    else if ((this.isAlpha(exp[index]) && (exp[index - 1] == ')')) && ((str == null) || !str.Equals("log")))
                    {
                        builder.Insert(index + num2, '*');
                        num2++;
                    }
                    else if ((exp[index] == '(') && (exp[index - 1] == ')'))
                    {
                        builder.Insert(index + num2, '*');
                        num2++;
                    }
                    else if (((exp[index] == '(') && this.isAlpha(exp[index - 1])) && (this.backTrack(exp.Substring(0, index)) == null))
                    {
                        builder.Insert(index + num2, '*');
                        num2++;
                    }
                }
                catch
                {
                }
                if (str != null)
                {
                    index += str.Length;
                }
                else
                {
                    index++;
                }
                str = null;
            }
            return builder.ToString();
        }

        private double sfac(double val)
        {
            if (!this.isInteger(val))
            {
                return double.NaN;
            }
            if (val < 0.0)
            {
                return double.NaN;
            }
            if (val <= 1.0)
            {
                return 1.0;
            }
            return (val * this.sfac(val - 2.0));
        }

        private string skipSpaces(string str)
        {
            int num = 0;
            int length = str.Length;
            StringBuilder builder = new StringBuilder(length);
            while (num < length)
            {
                if (str[num] != ' ')
                {
                    builder.Append(str[num]);
                }
                num++;
            }
            return builder.ToString();
        }

        private void Syntax(string exp)
        {
            int index = 0;
            int num2 = 0;
            string str = null;
            string str2 = null;
            if (!this.matchParant(exp))
            {
                throw new Exception("Non matching brackets");
            }
            int length = exp.Length;
            while (index < length)
            {
                try
                {
                    str = this.getOp(exp, index);
                    if (str != null)
                    {
                        num2 = str.Length;
                        index += num2;
                        str2 = this.getOp(exp, index);
                        if (((str2 != null) && this.isTwoArgOp(str2)) && (!str2.Equals("+") && !str2.Equals("-")))
                        {
                            throw new Exception("Syntax error near -> " + exp.Substring(index - num2));
                        }
                    }
                    else
                    {
                        if ((!this.isAlpha(exp[index]) && !this.isConstant(exp[index])) && !this.isAllowedSym(exp[index]))
                        {
                            throw new Exception("Syntax error near -> " + exp.Substring(index));
                        }
                        index++;
                    }
                    continue;
                }
                catch
                {
                    index++;
                    continue;
                }
            }
        }

        private double toValue(Node tree)
        {
            if (tree.getType() == Node.TYPE_CONSTANT)
            {
                return tree.getValue();
            }
            if (tree.getType() == Node.TYPE_VARIABLE)
            {
                string key = tree.getVariable();
                if (this.spconst.ContainsKey(key))
                {
                    return (double)this.spconst[key];
                }
                key = this.get(key);
                if (this.isConstant(key))
                {
                    return double.Parse(key);
                }
                this.Syntax(key);
                return this.toValue(this.parse(this.putMult(this.parseE(key))));
            }
            string str = tree.getOperator();
            Node node = tree.arg1();
            if (tree.arguments() == 2)
            {
                Node node2 = tree.arg2();
                if (str.Equals("+"))
                {
                    return (this.toValue(node) + this.toValue(node2));
                }
                if (str.Equals("-"))
                {
                    return (this.toValue(node) - this.toValue(node2));
                }
                if (str.Equals("*"))
                {
                    return (this.toValue(node) * this.toValue(node2));
                }
                if (str.Equals("/"))
                {
                    return (this.toValue(node) / this.toValue(node2));
                }
                if (str.Equals("^"))
                {
                    return Math.Pow(this.toValue(node), this.toValue(node2));
                }
                if (str.Equals("log"))
                {
                    return (Math.Log(this.toValue(node2)) / Math.Log(this.toValue(node)));
                }
                if (str.Equals("%"))
                {
                    return (this.toValue(node) % this.toValue(node2));
                }
                if (str.Equals("=="))
                {
                    if (this.toValue(node) != this.toValue(node2))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
                if (str.Equals("!="))
                {
                    if (this.toValue(node) == this.toValue(node2))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
                if (str.Equals("<"))
                {
                    if (this.toValue(node) >= this.toValue(node2))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
                if (str.Equals(">"))
                {
                    if (this.toValue(node) <= this.toValue(node2))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
                if (str.Equals("&&"))
                {
                    if ((this.toValue(node) == 1.0) && (this.toValue(node2) == 1.0))
                    {
                        return 1.0;
                    }
                    return 0.0;
                }
                if (str.Equals("||"))
                {
                    if ((this.toValue(node) != 1.0) && (this.toValue(node2) != 1.0))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
                if (str.Equals(">="))
                {
                    if (this.toValue(node) < this.toValue(node2))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
                if (str.Equals("<="))
                {
                    if (this.toValue(node) > this.toValue(node2))
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
            }
            else
            {
                double num;
                if (str.Equals("sqrt"))
                {
                    return Math.Sqrt(this.toValue(node));
                }
                if (str.Equals("sin"))
                {
                    return Math.Sin(this.toValue(node));
                }
                if (str.Equals("cos"))
                {
                    return Math.Cos(this.toValue(node));
                }
                if (str.Equals("tan"))
                {
                    return Math.Tan(this.toValue(node));
                }
                if (str.Equals("asin"))
                {
                    return Math.Asin(this.toValue(node));
                }
                if (str.Equals("acos"))
                {
                    return Math.Acos(this.toValue(node));
                }
                if (str.Equals("atan"))
                {
                    return Math.Atan(this.toValue(node));
                }
                if (str.Equals("ln"))
                {
                    return Math.Log(this.toValue(node));
                }
                if (str.Equals("exp"))
                {
                    return Math.Exp(this.toValue(node));
                }
                if (str.Equals("cotan"))
                {
                    return (1.0 / Math.Tan(this.toValue(node)));
                }
                if (str.Equals("acotan"))
                {
                    return (1.5707963267948966 - Math.Atan(this.toValue(node)));
                }
                if (str.Equals("ceil"))
                {
                    return Math.Ceiling(this.toValue(node));
                }
                if (str.Equals("round"))
                {
                    return Math.Round(this.toValue(node));
                }
                if (str.Equals("floor"))
                {
                    return Math.Floor(this.toValue(node));
                }
                if (str.Equals("fac"))
                {
                    return this.fac(this.toValue(node));
                }
                if (str.Equals("abs"))
                {
                    return Math.Abs(this.toValue(node));
                }
                if (str.Equals("fpart"))
                {
                    return this.fpart(this.toValue(node));
                }
                if (str.Equals("sfac"))
                {
                    return this.sfac(this.toValue(node));
                }
                if (str.Equals("sinh"))
                {
                    num = this.toValue(node);
                    return ((Math.Exp(num) - (1.0 / Math.Exp(num))) / 2.0);
                }
                if (str.Equals("cosh"))
                {
                    num = this.toValue(node);
                    return ((Math.Exp(num) + (1.0 / Math.Exp(num))) / 2.0);
                }
                if (str.Equals("tanh"))
                {
                    num = this.toValue(node);
                    return (((Math.Exp(num) - (1.0 / Math.Exp(num))) / 2.0) / ((Math.Exp(num) + (1.0 / Math.Exp(num))) / 2.0));
                }
                if (str.Equals("!"))
                {
                    if (this.toValue(node) == 1.0)
                    {
                        return 0.0;
                    }
                    return 1.0;
                }
            }
            throw new Exception("Unknown operator");
        }
    }

    public class Node
    {
        // Fields
        private Node _arg1;
        private Node _arg2;
        private string _operator;
        private int args;
        private int type;
        public static int TYPE_CONSTANT = 2;
        public static int TYPE_END = 4;
        public static int TYPE_EXPRESSION = 3;
        public static int TYPE_UNDEFINED = -1;
        public static int TYPE_VARIABLE = 1;
        private double value;
        private string variable;

        // Methods
        public Node(double value)
        {
            this._operator = "";
            this._arg1 = null;
            this._arg2 = null;
            this.args = 0;
            this.type = TYPE_UNDEFINED;
            this.value = double.NaN;
            this.variable = "";
            this.value = value;
            this.type = TYPE_CONSTANT;
        }

        public Node(string variable)
        {
            this._operator = "";
            this._arg1 = null;
            this._arg2 = null;
            this.args = 0;
            this.type = TYPE_UNDEFINED;
            this.value = double.NaN;
            this.variable = "";
            this.variable = variable;
            this.type = TYPE_VARIABLE;
        }

        public Node(string _operator, Node _arg1)
        {
            this._operator = "";
            this._arg1 = null;
            this._arg2 = null;
            this.args = 0;
            this.type = TYPE_UNDEFINED;
            this.value = double.NaN;
            this.variable = "";
            this._arg1 = _arg1;
            this._operator = _operator;
            this.args = 1;
            this.type = TYPE_EXPRESSION;
        }

        public Node(string _operator, Node _arg1, Node _arg2)
        {
            this._operator = "";
            this._arg1 = null;
            this._arg2 = null;
            this.args = 0;
            this.type = TYPE_UNDEFINED;
            this.value = double.NaN;
            this.variable = "";
            this._arg1 = _arg1;
            this._arg2 = _arg2;
            this._operator = _operator;
            this.args = 2;
            this.type = TYPE_EXPRESSION;
        }

        public Node arg1()
        {
            return this._arg1;
        }

        public Node arg2()
        {
            return this._arg2;
        }

        public int arguments()
        {
            return this.args;
        }

        public string getOperator()
        {
            return this._operator;
        }

        public int getType()
        {
            return this.type;
        }

        public double getValue()
        {
            return this.value;
        }

        public string getVariable()
        {
            return this.variable;
        }
    }

    public class Operator
    {
        // Fields
        private int args = 0;
        private string op = "";
        private int prec = 0x7fffffff;

        // Methods
        public Operator(string _operator, int arguments, int precedence)
        {
            this.op = _operator;
            this.args = arguments;
            this.prec = precedence;
        }

        public int arguments()
        {
            return this.args;
        }

        public string getOperator()
        {
            return this.op;
        }

        public int precedence()
        {
            return this.prec;
        }
    }
}
