using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sample05
{
    class Program
    {

        private const string sql_conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=CoreSample;
            Integrated Security=True;MultipleActiveResultSets=True;";
        private const string sql_insert = @"Insert into [dbo].[content] (title,[content],status,add_time,modify_time) 
            values(@title,@content,@status,@add_time,@modify_time)";
        private const string sql_del = @"delete from [dbo].[content] where (id = @id)";
        private const string sql_update = @"update [dbo].[content] set title = @title, [content] = @content,
                    modify_time = Getdate() where (id = @id)";

        static void Main(string[] args)
        {
            //test_insert();
            //test_mult_insert();
            //test_del();
            //test_mult_del();
            //test_update();
            //test_mult_update();
            //test_select_one();
            //test_select_lsit();
            test_select_content_with_comment();
            Console.ReadLine();
        }

        static void test_insert()
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1"
            };

            using (var conn = new SqlConnection(sql_conn))
            {
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert:插入了{result}条数据");
            }
        }

        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    title = "批量插入标题1",
                    content = "批量插入内容1"
                },
                new Content
                {
                    title = "批量插入标题2",
                    content = "批量插入内容2"
                }
            };

            using (var conn = new SqlConnection(sql_conn))
            {
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert:插入了{result}条数据");
            }
        }

        static void test_del()
        {
            var content = new Content
            {
                id = 2,
            };

            using (var conn = new SqlConnection(sql_conn))
            {
                var result = conn.Execute(sql_del, content);
                Console.WriteLine($"test_del:删除了{result}条数据");
            }
        }

        static void test_mult_del()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    id=3
                },
                new Content{ id = 4}
            };

            using (var conn = new SqlConnection(sql_conn))
            {
                var result = conn.Execute(sql_del, contents);
                Console.WriteLine($"test_mult_del:删除了{result}条数据");
            }
        }

        static void test_update()
        {
            var content = new Content
            {
                id = 5,
                title = "标题5",
                content = "内容5",
            };

            using (var conn = new SqlConnection(sql_conn))
            {
                var result = conn.Execute(sql_update, content);
                Console.WriteLine($"test_update:修改了{result}条数据");
            }
        }

        static void test_mult_update()
        {
            List<Content> contents = new List<Content>()
            {
                new Content { id = 6, title = "批量修改标题6", content = "批量修改内容6"},
                new Content { id = 7, title = "批量修改标题7", content = "批量修改内容7"}
            };

            using (var conn = new SqlConnection(sql_conn))
            {
                var result = conn.Execute(sql_update, contents);
                Console.WriteLine($"test_mult_update:修改了{result}条数据");
            }
        }

        static void test_select_one()
        {
            using (var conn = new SqlConnection(sql_conn))
            {
                string sql_select = @"select * from [dbo].[content] where id = @id";
                var result = conn.QueryFirstOrDefault<Content>(sql_select, new { id = 5 });
                Console.WriteLine($"test_select_one:查到了1条数据:");
            }
        }

        static void test_select_lsit()
        {
            using (var conn = new SqlConnection(sql_conn))
            {
                string sql_select = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<Content>(sql_select, new { ids = new int[] { 6, 7 } });
                Console.WriteLine($"test_select_list:查到了{result.AsList().Count}条数据");
            }
        }

        static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection(sql_conn))
            {
                string sql_with = @"select * from content where id = @id;
                    select * from comment where content_id = @id";
                using (var result = conn.QueryMultiple(sql_with, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<ContentWithComment>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量:" +
                        $"{content.comments.AsList().Count}");
                }
            }
        }
    }
}
