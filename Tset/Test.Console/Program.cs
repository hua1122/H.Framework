using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Expressions;
using Test.Console.Model;

namespace Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sql = ExpressionsHelper.GetSqlFromExpression<Nurse>(w => w.Id > 0);
            System.Console.WriteLine(sql);
            System.Console.ReadKey();
        }
    }
}
