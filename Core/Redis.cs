using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using ServiceStack.Redis;

namespace EviCRM.Server.Core
{
    public class Redis
    {
        const string redis_host_str = "82.146.44.138";
        const string redis_port_str = "6379";
        const string redis_password = "512Dcfbd28#";

        // GET: Counter    

        public bool Redis_PongTest()
        {
            string test_phrase = Redis_GetByKey("PING");
            if (test_phrase == "PONG")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public long Redis_IncrementByKey(string key,uint step)
        {
            var host = redis_host_str;
            var port = Convert.ToInt32(redis_port_str);
            RedisEndpoint redisEndpoint = new RedisEndpoint(host, port,redis_password);

            using (var client = new RedisClient(redisEndpoint))
            {
                return client.Increment(key, step);
            }

            return long.MinValue;
        }

        public string Redis_GetByKey(string key)
        {
            var host = redis_host_str;
            var port = Convert.ToInt32(redis_port_str);
            RedisEndpoint redisEndpoint = new RedisEndpoint(host, port,redis_password);

            using (var client = new RedisClient(redisEndpoint))
            {
                Console.WriteLine("Redis: (get by key) : " + client.Get(key));
                return convert_bytearr_to_string(client.Get(key));
            }

            return "";
        }

        public List<string> Redis_GetKeysByMask(string keys_mask)
        {
            var host = redis_host_str;
            var port = Convert.ToInt32(redis_port_str);
            RedisEndpoint redisEndpoint = new RedisEndpoint(host, port,redis_password);

            using (var client = new RedisClient(redisEndpoint))
            {
                Console.WriteLine("Redis: (get keys by mask) : ");
                foreach (string elem in client.GetKeysByPattern(keys_mask).ToList())
                {
                    Console.WriteLine("Redis: " + elem);
                }
                return client.GetKeysByPattern(keys_mask).ToList();
            }

            return null;
        }

        public byte[] convert_string_to_bytearr(string inp)
        {
            return Encoding.ASCII.GetBytes(inp);
        }

        public string convert_bytearr_to_string(byte[] arr)
        {
            return System.Text.Encoding.UTF8.GetString(arr);
        }
        
        public string Redis_SetByKey(string key,string value)
        {
            var host = redis_host_str;
            var port = Convert.ToInt32(redis_port_str);
            RedisEndpoint redisEndpoint = new RedisEndpoint(host, port,redis_password);

            using (var client = new RedisClient(redisEndpoint))
            {
                Console.WriteLine("Redis: (set by key) : " + "[key : " + key + " ] , [value : " + value + " ]");
                return client.Set(key,convert_string_to_bytearr(value)).ToString();
            }

            return "";
        }

        public List<string> Redis_GetAllKeys()
        {
            var host = redis_host_str;
            var port = Convert.ToInt32(redis_port_str);
            RedisEndpoint redisEndpoint = new RedisEndpoint(host, port,redis_password);

            using (var client = new RedisClient(redisEndpoint))
            {
                Console.WriteLine("Redis: (get all keys) : ");

                foreach(string elem in client.GetAllKeys())
                {
                    Console.WriteLine(elem);
                }

                return client.GetAllKeys();
            }

            return null;
        }


    }
}
