using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace course_work
{
    public class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public LR(List<Token> enterToken)
        {
            tokens = enterToken;
        }

        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }

        private void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }

        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }

        private void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }

        private void Expression()
        {
            Deycstra deycstra = new Deycstra();
            while (GetLexeme(nextLex).Type != TokenType.SEMICOLON && GetLexeme(nextLex).Type != TokenType.LINEBREAK)
            {
                if (GetLexeme(nextLex).Type == TokenType.LINEBREAK)
                    throw new Exception($"Ожидалось терминал ';'");
                deycstra.TakeToken(GetLexeme(nextLex));
                Shift();
            }
            Token k = new Token(TokenType.EXPR);
            lexemStack.Push(k);
            deycstra.StartOPN();
        }

        private void State0()
        {
            if (lexemStack.Count == 0)
                Shift();

            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            if (nextLex == tokens.Count)
                                isEnd = true;
                            break;
                        default:
                            throw new Exception($"State0\n String: {nextLex} Ожидалось нетерминал <программа>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.MAIN:
                    GoToState(1);
                    break;
                default:
                    throw new Exception($"State0\n String: {nextLex} Ожидалось терминал MAIN но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.MAIN:
                    Shift();
                    break;
                case TokenType.LPAR:
                    GoToState(2);
                    break;
                default:
                    throw new Exception($"State1\n String: {nextLex} Ожидалось LPAR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State2()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LPAR:
                    Shift();
                    break;
                case TokenType.RPAR:
                    GoToState(3);
                    break;
                default:
                    throw new Exception($"State2\n String: {nextLex} Ожидалось RPAR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State3()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.RPAR:
                    Shift();
                    break;
                case TokenType.LCURBRACE:
                    GoToState(4);
                    break;
                default:
                    throw new Exception($"State3\n String: {nextLex} Ожидалось LCURBRACE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State4()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            GoToState(5);
                            break;
                        case "<опис>":
                            GoToState(6);
                            break;
                        case "<тип>":
                            GoToState(7);
                            break;
                        default:
                            throw new Exception($"State4\n String: {nextLex} Ожидалось нетерминал <спис_опис>, <опис>, <тип>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.LCURBRACE:
                    Shift();
                    break;

                case TokenType.INT:
                    GoToState(8);
                    break;

                case TokenType.CHAR:
                    GoToState(9);
                    break;

                case TokenType.STRING:
                    GoToState(10);
                    break;

                default:
                    throw new Exception($"State4\n String: {nextLex} Ожидалось терминал INT, BOOL, STRING, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State5()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            Shift();
                            break;
                        case "<спис_опер>":
                            GoToState(11);
                            break;
                        case "<опер>":
                            GoToState(62);
                            break;
                        case "<условн>":
                            GoToState(13);
                            break;
                        case "<присв>":
                            GoToState(14);
                            break;
                        case "<опис>":
                            GoToState(17);
                            break;
                        case "<тип>":
                            GoToState(7);
                            break;

                        default:
                            throw new Exception($"State5\n String: {nextLex} Ожидалось нетерминал <спис_опис>, <спис_опер>, <опер>, <условн>, <присв> а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.INT:
                    GoToState(8);
                    break;

                case TokenType.CHAR:
                    GoToState(9);
                    break;

                case TokenType.STRING:
                    GoToState(10);
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                default:
                    throw new Exception($"State5\n String: {nextLex} Ожидалось терминал IF, ID, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State6()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опис>")
                Reduce(1, "<спис_опис>");
            else
                throw new Exception($"State6\n String: {nextLex} Ожидалось нетерминал <опис>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State7()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            Shift();
                            break;

                        case "<спис_перем>":
                            GoToState(18);
                            break;
                        default:
                            throw new Exception($"State7\n String: {nextLex} Ожидалось <тип>, <спис_перем>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.ID:
                    GoToState(19);
                    break;

                default:
                    throw new Exception($"State7\n String: {nextLex} Ожидалось терминал ID (ID), но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State8()
        {
            if (lexemStack.Peek().Type == TokenType.INT)
                Reduce(1, "<тип>");
            else
                throw new Exception($"State8\n String: {nextLex} Ожидалось терминал INT, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State9()
        {
            if (lexemStack.Peek().Type == TokenType.CHAR)
                Reduce(1, "<тип>");
            else
                throw new Exception($"State9\n String: {nextLex} Ожидалось терминал BOOL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State10()
        {
            if (lexemStack.Peek().Type == TokenType.STRING)
                Reduce(1, "<тип>");
            else
                throw new Exception($"State10\n String: {nextLex} Ожидалось терминал STRING, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State11()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            Shift();
                            break;

                        case "<опер>":
                            GoToState(62);
                            break;

                        case "<опис>":
                            GoToState(17);
                            break;

                        case "<тип>":
                            GoToState(7);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;

                        default:
                            throw new Exception($"State11\n String: {nextLex} Ожидалось нетерминал <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.RCURBRACE:
                    GoToState(20);
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.INT:
                    GoToState(8);
                    break;

                case TokenType.CHAR:
                    GoToState(9);
                    break;

                case TokenType.STRING:
                    GoToState(10);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                default:
                    throw new Exception($"State11\n String: {nextLex} Ожидалось терминал закрывающуюся фигурную скобку, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State12()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LPAR:
                    GoToState(15);
                    break;

                case TokenType.FOR:
                    Shift();
                    break;

                default:
                    throw new Exception($"State12\n String: {nextLex} Ожидалось терминал '(', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State13()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<условн>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"State13\n String: {nextLex} Ожидалось нетерминал <условн>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State14()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<присв>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"State14\n String: {nextLex} Ожидалось нетерминал <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State15()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            GoToState(22);
                            break;

                        default:
                            throw new Exception($"State11\n String: {nextLex} Ожидалось нетерминал <тип>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    }
                    break;

                case TokenType.ID:
                    GoToState(26);
                    break;

                case TokenType.LPAR:
                    Shift();
                    break;

                default:
                    throw new Exception($"State11\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State16() //КОНФЛИКТ
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ID:
                    Shift();
                    break;

                case TokenType.EQUAL:
                    GoToState(23);
                    break;

                default:
                    throw new Exception($"State16\n String: {nextLex} Ожидалось терминал '=', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State17()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опис>")
                Reduce(2, "<спис_опис>");
            else
                throw new Exception($"State17\n String: {nextLex} Ожидалось нетерминал <опис>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State18()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_перем>":
                            Shift();
                            break;

                        default:
                            throw new Exception($"State18\n String: {nextLex} Ожидалось нетерминал <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.SEMICOLON:
                    GoToState(24);
                    break;

                case TokenType.COMMA:
                    GoToState(25);
                    break;

                default:
                    throw new Exception($"State18\n String: {nextLex} Ожидалось терминал ';', ',', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State19()
        {
            if (lexemStack.Peek().Type == TokenType.ID)
                Reduce(1, "<спис_перем>");
            else
                throw new Exception($"State19\n String: {nextLex} Ожидалось терминал ID, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State20()
        {
            if (lexemStack.Peek().Type == TokenType.RCURBRACE)
                Reduce(7, "<программа>");
            else
                throw new Exception("State20\n String: {nextLex} Ожидалось терминал открывающаяся фигурная скобка, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        public void State21()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(2, "<спис_опер>");
            else
                throw new Exception($"State21\n String: {nextLex} Ожидалось нетерминал <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }


        private void State22()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            Shift();
                            break;
                    }
                    break;

                case TokenType.ID:
                    GoToState(27);
                    break;

                default:
                    throw new Exception($"State23\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State23()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            GoToState(29);
                            break;

                        default:
                            throw new Exception($"State23\n String: {nextLex} Ожидалось нетерминал <операнд>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.EQUAL:
                    Expression();
                    break;

                case TokenType.LIT:/*тут были комменты на лит и айди*/
                    GoToState(31);
                    break;

                case TokenType.ID:
                    GoToState(30);
                    break;

                case TokenType.EXPR:
                    GoToState(33);
                    break;

                default:
                    throw new Exception($"State23\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, ЛИТЕРАЛ, EXPR но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State24()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(3, "<опис>");
            else
                throw new Exception($"State24\n String: {nextLex} Ожидалось терминал ';', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State25()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COMMA:
                    Shift();
                    break;

                case TokenType.ID:
                    GoToState(32);
                    break;

                default:
                    throw new Exception($"State25\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State26()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    GoToState(28);
                    break;

                case TokenType.ID:
                    Shift();
                    break;

                default:
                    throw new Exception($"State26\n String: {nextLex} Ожидалось терминал '=', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State27()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    GoToState(34);
                    break;

                case TokenType.ID:
                    Shift();
                    break;

                default:
                    throw new Exception($"State27\n String: {nextLex} Ожидалось терминал '=', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State28()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            GoToState(35);
                            break;

                        default:
                            throw new Exception($"State28\n String: {nextLex} Ожидалось нетерминал <операнд>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.ID:
                    GoToState(30);
                    break;

                case TokenType.LIT:
                    GoToState(31);
                    break;

                case TokenType.EQUAL:
                    Shift();
                    break;

                default:
                    throw new Exception($"State28\n String: {nextLex} Ожидалось терминал '=', ИДЕНТИФИКАТОР, ЛИТЕРАЛ, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State29()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                    }
                    break;

                case TokenType.SEMICOLON:
                    GoToState(41);
                    break;
                    
                default:
                    throw new Exception($"State29\n String: {nextLex} Ожидалось терминал ;, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State30()
        {
            if (lexemStack.Peek().Type == TokenType.ID)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"State30\n String: {nextLex} Ожидалось терминал ID, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State31()
        {
            if (lexemStack.Peek().Type == TokenType.LIT)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"State31\n String: {nextLex} Ожидалось терминал LIT, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State32()
        {
            if (lexemStack.Peek().Type == TokenType.ID)
                Reduce(3, "<спис_перем>");
            else
                throw new Exception($"State32\n String: {nextLex} Ожидалось нетерминал <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State33()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EXPR:
                    Shift();
                    break;

                case TokenType.SEMICOLON:
                    GoToState(42);
                    break;

                default:
                    throw new Exception($"State29\n String: {nextLex} Ожидалось терминал ;, +, -, /, *, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State34()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            GoToState(36);
                            break;

                        default:
                            throw new Exception($"State34\n String: {nextLex} Ожидалось нетерминал <операнд>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.EQUAL:
                    Shift();
                    break;

                case TokenType.ID:
                    GoToState(30);
                    break;

                case TokenType.LIT:
                    GoToState(31);
                    break;

                default:
                    throw new Exception($"State34\n String: {nextLex} Ожидалось терминал LCURBRACE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State35()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                       }
                    break;

                case TokenType.SEMICOLON:
                    GoToState(37);
                    break;

                default:
                    throw new Exception($"State35\n String: {nextLex} Ожидалось терминал ';', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State36()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                    }
                    break;

                case TokenType.SEMICOLON:
                    GoToState(38);
                    break;

                default:
                    throw new Exception($"State36\n String: {nextLex} Ожидалось терминал ';', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State37()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<лог_опер>":
                            GoToState(54);
                            break;

                        default:
                            throw new Exception($"State37\n String: {nextLex} Ожидалось нетерминал <лог_опер>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.SEMICOLON:
                    Shift();
                    break;

                case TokenType.ID:
                    GoToState(39);
                    break;

                default:
                    throw new Exception($"State37\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State38()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<лог_опер>":
                            GoToState(46);
                            break;

                        default:
                            throw new Exception($"State38\n String: {nextLex} Ожидалось нетерминал <лог_опер>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.SEMICOLON:
                    Shift();
                    break;

                case TokenType.ID:
                    GoToState(39);
                    break;

                default:
                    throw new Exception($"State38\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State39()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак2>":
                            GoToState(40);
                            break;

                        default:
                            throw new Exception($"State39\n String: {nextLex} Ожидалось нетерминал <знак2>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.LESS:
                    GoToState(44);
                    break;

                case TokenType.MORE:
                    GoToState(43);
                    break;

                case TokenType.ID:
                    Shift();
                    break;

                default:
                    throw new Exception($"State39\n String: {nextLex} Ожидалось терминал '>', '<', ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State40()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак2>":
                            Shift();
                            break;

                        case "<операнд>":
                            GoToState(45);
                            break;

                        default:
                            throw new Exception($"State40\n String: {nextLex} Ожидалось нетерминал <операнд>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.ID:
                    GoToState(30);
                    break;

                case TokenType.LIT:
                    GoToState(31);
                    break;

                default:
                    throw new Exception($"State40\n String: {nextLex} Ожидалось терминал ИДЕНТИФИКАТОР, ЛИТЕРАЛ, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State41()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<присв>");
            else
                throw new Exception($"State41\n SEMICOLON");
        }

        private void State42()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<присв>");
            else
                throw new Exception($"State42\n SEMICOLON");
        }

        private void State43()
        {
            if (lexemStack.Peek().Type == TokenType.MORE)
                Reduce(1, "<знак2>");
            else
                throw new Exception($"State43\n SEMICOLON");
        }

        private void State44()
        {
            if (lexemStack.Peek().Type == TokenType.LESS)
                Reduce(1, "<знак2>");
            else
                throw new Exception($"State44\n SEMICOLON");
        }

        private void State45()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<операнд>")
                Reduce(3, "<лог_опер>");
            else
                throw new Exception($"State45\n String: {nextLex} Ожидалось нетерминал <операнд>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State46()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<лог_опер>":
                            Shift();
                            break;
                    }
                    break;

                case TokenType.SEMICOLON:
                    GoToState(47);
                    break;

                default:
                    throw new Exception($"State40\n String: {nextLex} Ожидалось терминал ';', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State47()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<2опер>":
                            GoToState(53);
                            break;

                        default:
                            throw new Exception($"State40\n String: {nextLex} Ожидалось нетерминал <2опер>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.ID:
                    GoToState(48);
                    break;

                case TokenType.SEMICOLON:
                    Shift();
                    break;

                default:
                    throw new Exception($"State47\n Ожидалось терминал ИДЕНТИФИКАТОР, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }
        private void State48()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак>":
                            GoToState(51);
                            break;

                        default:
                            throw new Exception($"State48\n String: {nextLex} Ожидалось нетерминал <знак>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.ID:
                    Shift();
                    break;

                case TokenType.PLUS:
                    GoToState(49);
                    break;

                case TokenType.MINUS:
                    GoToState(50);
                    break;

                default:
                    throw new Exception($"State48\n ID Ожидалось терминал '+', '-', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }
        private void State49()
        {
            if (lexemStack.Peek().Type == TokenType.PLUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"State49\n String: {nextLex} Ожидалось терминал '+', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State50()
        {
            if (lexemStack.Peek().Type == TokenType.MINUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"State50\n String: {nextLex} Ожидалось терминал '-', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State51() //**
        {

            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак_доп>":
                            GoToState(52);
                            break;

                        case "<знак>":
                            Shift();
                            break;

                        default:
                            throw new Exception($"State51\n String: {nextLex} Ожидалось нетерминал <знак_доп>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;


                case TokenType.PLUS:
                    GoToState(69);
                    break;

                case TokenType.MINUS:
                    GoToState(70);
                    break;

                default:
                    throw new Exception($"State51\n Ожидалось терминал '+', '-', но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State52()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<знак_доп>")
                Reduce(3, "<2опер>");
            else
                throw new Exception($"State52\n String: {nextLex} Ожидалось нетерминал <знак_доп>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State53()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<2опер>":
                            Shift();
                            break;
                    }
                    break;

                case TokenType.RPAR:
                    GoToState(57);
                    break;

                default:
                    throw new Exception($"State53\n ')'");
            }
        }

        private void State54()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<лог_опер>":
                            Shift();
                            break;
                    }
                    break;

                case TokenType.SEMICOLON:
                    GoToState(55);
                    break;

                default:
                    throw new Exception($"State54\n ';'");
            }
        }

        private void State55()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<2опер>":
                            GoToState(56);
                            break;

                        default:
                            throw new Exception($"State55\n String: {nextLex} Ожидалось нетерминал <2опер>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.SEMICOLON:
                    Shift();
                    break;

                case TokenType.ID:
                    GoToState(48);
                    break;

                default:
                    throw new Exception($"State55\n ИДЕНТИФИКАТОР");
            }
        }

        private void State56()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<2опер>":
                            Shift();
                            break;

                        default:
                            throw new Exception($"State55\n String: {nextLex} Ожидалось нетерминал <2опер>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.RPAR:
                    GoToState(58);
                    break;

                default:
                    throw new Exception($"State55\n ')'");
            }
        }

        private void State57()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер>":
                            GoToState(67);
                            break;

                        case "<опер>":
                            GoToState(59);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;

                        default:
                            throw new Exception($"State57\n ");
                    }
                    break;

                case TokenType.RPAR:
                    Shift();
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                case TokenType.RCURBRACE:
                    GoToState(60);
                    break;

                default:
                    throw new Exception($"State57\n ID");
            }
        }

        private void State58() //**
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер>":
                            GoToState(68);
                            break;

                        case "<опер>":
                            GoToState(59);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;

                        default:
                            throw new Exception($"State58\n Ожидалось нетерминал <блок_опер>, <опер>, <условн>, <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.RPAR:
                    Shift();
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                case TokenType.LCURBRACE:
                    GoToState(61); 
                    break;

                default:
                    throw new Exception($"State58\n Ожидалось терминал LCURBRACE, ID, FOR но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State59()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(1, "<блок_опер>");
            else
                throw new Exception($"State59\n String: {nextLex} Ожидалось нетерминал <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State60()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(63);
                            break;

                        case "<опер>":
                            GoToState(62);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;

                        default:
                            throw new Exception($"State60\n  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.LCURBRACE:
                    Shift();
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                default:
                    throw new Exception($"State60\n  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State61()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(64);
                            break;

                        case "<опер>":
                            GoToState(62);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;

                        default:
                            throw new Exception($"State61\nString: {nextLex} Ожидалось нетерминал ..., но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.LCURBRACE:
                    Shift();
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                default:
                    throw new Exception($"State61\nString: {nextLex} Ожидалось терминал ..., но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} ");
            }
        }

        private void State62()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(1, "<спис_опер>");
            else
                throw new Exception($"State62\n String: {nextLex} Ожидалось нетерминал <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State63()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            Shift();
                            break;
                        case "<опер>":
                            GoToState(21);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;
                    }
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                case TokenType.RCURBRACE:
                    GoToState(65);
                    break;

                default:
                    throw new Exception($"State63\nString: {nextLex} Ожидалось терминал RCURBRACE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} ");
            }
        }

        private void State64() // 123
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            Shift();
                            break;
                        case "<опер>":
                            GoToState(21);
                            break;

                        case "<условн>":
                            GoToState(13);
                            break;

                        case "<присв>":
                            GoToState(14);
                            break;

                        default:
                            throw new Exception($"State11\n String: {nextLex} Ожидалось нетерминал <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.RCURBRACE:
                    GoToState(66);
                    break;

                case TokenType.FOR:
                    GoToState(12);
                    break;

                case TokenType.ID:
                    GoToState(16);
                    break;

                default:
                    throw new Exception($"State11\n String: {nextLex} Ожидалось нетерминал <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State65()
        {
            if (lexemStack.Peek().Type == TokenType.RCURBRACE)
                Reduce(3, "<блок_опер>");
            else
                throw new Exception($"State65\n String: {nextLex} Ожидалось терминал RCURBRACE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State66()
        {
            if (lexemStack.Peek().Type == TokenType.RCURBRACE)
                Reduce(3, "<блок_опер>");
            else
                throw new Exception($"State66\n String: {nextLex} Ожидалось терминал RCURBRACE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        public void State67()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок_опер>")
                Reduce(12, "<условн>");
            else
                throw new Exception($"State67\n String: {nextLex} Ожидалось нетерминал <блок_опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        public void State68()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок_опер>")
                Reduce(11, "<условн>");
            else
                throw new Exception($"State68\n String: {nextLex} Ожидалось нетерминал <блок_опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State69()
        {
            if (lexemStack.Peek().Type == TokenType.PLUS)
                Reduce(1, "<знак_доп>");
            else
                throw new Exception($"State69\n String: {nextLex} Ожидалось терминал PLUS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State70()
        {
            if (lexemStack.Peek().Type == TokenType.MINUS)
                Reduce(3, "<знак_доп>");
            else
                throw new Exception($"State70\n String: {nextLex} Ожидалось терминал MINUS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        public void Start()
        {
            try
            {
                stateStack.Push(0);
                while (isEnd != true)
                    switch (state)
                    {
                        case 0:
                            State0();
                            break;
                        case 1:
                            State1();
                            break;
                        case 2:
                            State2();
                            break;
                        case 3:
                            State3();
                            break;
                        case 4:
                            State4();
                            break;
                        case 5:
                            State5();
                            break;
                        case 6:
                            State6();
                            break;
                        case 7:
                            State7();
                            break;
                        case 8:
                            State8();
                            break;
                        case 9:
                            State9();
                            break;
                        case 10:
                            State10();
                            break;
                        case 11:
                            State11();
                            break;
                        case 12:
                            State12();
                            break;
                        case 13:
                            State13();
                            break;
                        case 14:
                            State14();
                            break;
                        case 15:
                            State15();
                            break;
                        case 16:
                            State16();
                            break;
                        case 17:
                            State17();
                            break;
                        case 18:
                            State18();
                            break;
                        case 19:
                            State19();
                            break;
                        case 20:
                            State20();
                            break;
                        case 21:
                            State21();
                            break;
                        case 22:
                            State22();
                            break;
                        case 23:
                            State23();
                            break;
                        case 24:
                            State24();
                            break;
                        case 25:
                            State25();
                            break;
                        case 26:
                            State26();
                            break;
                        case 27:
                            State27();
                            break;
                        case 28:
                            State28();
                            break;
                        case 29:
                            State29();
                            break;
                        case 30:
                            State30();
                            break;
                        case 31:
                            State31();
                            break;
                        case 32:
                            State32();
                            break;
                        case 33:
                            State33();
                            break;
                        case 34:
                            State34();
                            break;
                        case 35:
                            State35();
                            break;
                        case 36:
                            State36();
                            break;
                        case 37:
                            State37();
                            break;
                        case 38:
                            State38();
                            break;
                        case 39:
                            State39();
                            break;
                        case 40:
                            State40();
                            break;
                        case 41:
                            State41();
                            break;
                        case 42:
                            State42();
                            break;
                        case 43:
                            State43();
                            break;
                        case 44:
                            State44();
                            break;
                        case 45:
                            State45();
                            break;
                        case 46:
                            State46();
                            break;
                        case 47:
                            State47();
                            break;
                        case 48:
                            State48();
                            break;
                        case 49:
                            State49();
                            break;
                        case 50:
                            State50();
                            break;
                        case 51:
                            State51();
                            break;
                        case 52:
                            State52();
                            break;
                        case 53:
                            State53();
                            break;
                        case 54:
                            State54();
                            break;
                        case 55:
                            State55();
                            break;
                        case 56:
                            State56();
                            break;
                        case 57:
                            State57();
                            break;
                        case 58:
                            State58();
                            break;
                        case 59:
                            State59();
                            break;
                        case 60:
                            State60();
                            break;
                        case 61:
                            State61();
                            break;
                        case 62:
                            State62();
                            break;
                        case 63:
                            State63();
                            break;
                        case 64:
                            State64();
                            break;
                        case 65:
                            State65();
                            break;
                        case 66:
                            State66();
                            break;
                        case 67:
                            State67();
                            break;
                        case 68:
                            State68();
                            break;
                        case 69:
                            State69();
                            break;
                        case 70:
                            State70();
                            break;
                    }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                MessageBox.Show($"Error! {ex.Message}");
            }
        }
    }
}
