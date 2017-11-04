using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, bool> where = x =>
            {
                if (x % 3 == 0)
                {
                    Console.WriteLine("{0} % 3 = True", x);
                }
                return x % 3 == 0;
            };

            Func<int, int> select = x =>
            {
                if (x % 2 == 0)
                {
                    Console.WriteLine("{0} ^ 2 = {1}", x, x * x);
                }
                else
                {
                    Console.WriteLine("{0} ^ 2 + 1 = {1}", x, x * x + 1);
                }
                return x % 2 == 0 ? x * x : x * x + 1;
            };

            Func<int, int> order = x =>
            {
                Console.WriteLine("OrderByDecending{0}", x);
                return x;
            };

            Func<int, bool> firstOrDefault = x =>
            {
                if (x == 81)
                {
                    Console.WriteLine("x = 81!!");
                }
                return x == 81;
            };

            var list = Enumerable.Range(1, 20);

            // Q.1 foreach Start~EndまでのConsole出力結果はどうなりますか？
            var query = list.Where(where).Select(select).OrderByDescending(order);
            Console.WriteLine("foreach Start!!");
            foreach (var x in query)
            {
                Console.WriteLine("Result = {0}", x);
            }
            Console.WriteLine("foreach End!!");
            Console.WriteLine("\n\n\n");
            #region Point
            // ・メソッドの呼び出し順を確認する。(遅延実行とyield retunの確認)。
            // 　※whereメソッドをループして、Selectメソッドをループして…ではなく
            //     whereメソッド→Selectメソッド→…とループしていること
            //     処理はforeachが行われてから実行されることを確認する。
            // https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/Where.cs
            // .netCoreのWhereのソース72行目 yield returnしていることを確認する。
            // またList<T>の拡張メソッドではなくEnumerableの拡張メソッドとして定義され、List<T>などはEnumerableの親クラスのメソッドを使用していることを確認する。
            #endregion

            // Q.2 FirstOrDefault Start~Endまでの出力結果はどうなりますか？
            // また、foreachの記載が不要な理由は？
            Console.WriteLine("FirstOrDefault Start!!");
            var result = list.Where(where).Select(select).OrderByDescending(order).FirstOrDefault(firstOrDefault);

            if (result != default(int))
            {
                Console.WriteLine("Find!!");
            }
            Console.WriteLine("FirstOrDefault End!!");

            Console.WriteLine("\n\n\n");
            #region Point
            // ・Count,Sum,ToArray,Average,ToList…などのメソッドの違い(foreach の呼び出し有無)を確認する。
            // https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/Sum.cs
            // .netCoreのSumのソース21行目参照
            #endregion

            // Q.3 Count Start~Endまでの出力結果はどうなりますか？
            Console.WriteLine("Count Start!!");
            Console.WriteLine("Count1 Start!!");
            var countResult1 = list.OrderBy(order).Select(select).Where(where).Count();
            Console.WriteLine("Count1 End!!\n");

            Console.WriteLine("Count2 Start!!");
            var countResult2 = list.Select(select).Where(where).OrderBy(order).Count();
            Console.WriteLine("Count2 End!!\n");

            Console.WriteLine("Count3 Start!!");
            var countResult3 = list.Where(where).Select(select).OrderByDescending(order).Count();
            Console.WriteLine("Count3 End!!\n");

            Console.WriteLine("countResult1 = {0}", countResult1);
            Console.WriteLine("countResult2 = {0}", countResult2);
            Console.WriteLine("countResult3 = {0}", countResult3);
            Console.WriteLine("Count End!!");
            #region Point
            // ・メソッド呼び出し順により結果がわかること
            // ・実施したい実装によって、メソッドの呼び出す順番は重要なことを確認
            #endregion


            Console.ReadLine();
        }
    }
}
