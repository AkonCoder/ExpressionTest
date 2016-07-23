using System;
using System.Linq.Expressions;

namespace ExpressionTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //1.基本的Expresion
            Expression<Func<int, int>> exp = x => x + 1;
            Console.WriteLine(exp.ToString());


            //2.表达式的定义和表现形式
            //编辑不通过
            //Expression<Func<int, int, int>> expressionTwo = (x, y) => { return x + y; };
            //Expression<Action<int>> expressionThree = x => { };


            //3.Expression转换，expression继承于LambdaExpression
            var lambdaExp = exp as LambdaExpression;
            Console.WriteLine(lambdaExp.Body);
            Console.WriteLine(lambdaExp.ReturnType);


            //4.遍历Expression里面的paramaters
            foreach (var item in lambdaExp.Parameters)
            {
                Console.WriteLine("Name:{0},Type is {1}", item.Name, item.Type);
            }

            //5.复杂表达式树
            LabelTarget labelBreak = Expression.Label();
            ParameterExpression loopIndex = Expression.Parameter(typeof(int), "index");

            BlockExpression block = Expression.Block(
            new[] { loopIndex },
                // 初始化loopIndex =1
                Expression.Assign(loopIndex, Expression.Constant(1)),
                Expression.Loop(
                    Expression.IfThenElse(
                // if 的判断逻辑
                        Expression.LessThanOrEqual(loopIndex, Expression.Constant(10)),
                // 判断逻辑通过的代码
                        Expression.Block(
                            Expression.Call(
                                null,
                                typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                Expression.Constant("Hello")),
                            Expression.PostIncrementAssign(loopIndex)),
                // 判断不通过的代码
                        Expression.Break(labelBreak)
                        ), labelBreak));

            // 将我们上面的代码块表达式
            Expression<Action> lambdaExpression = Expression.Lambda<Action>(block);
            lambdaExpression.Compile().Invoke();


            //6.表达式树的拓展

            Console.ReadLine();
        }
    }
}