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

    public static RedisInstance getInstance()
    {
        if (_instance == null)
        {
            _instance = new RedisInstance();
        }
        return _instance;

    }
}