using StackExchange.Redis;

public sealed class RedisInstance
{

    private static RedisInstance _instance;
    public ConnectionMultiplexer redis;

    private RedisInstance()
    {
        redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { "localhost:6379" },
        });
    }

    public static RedisInstance GetInstance()
    {
        if (_instance == null)
        {
            _instance = new RedisInstance();
        }
        return _instance;
    }

    public static IDatabase GetRedisDatabase(){
        return GetInstance().redis.GetDatabase();
    }

    public static IServer GetRedisServer(){
        return GetInstance().redis.GetServer("localhost", 6379);
    }
}