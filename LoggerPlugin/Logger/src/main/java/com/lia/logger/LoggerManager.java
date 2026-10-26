package com.lia.logger;

import android.util.Log;

public class LoggerManager {
    private static final LoggerManager instance =  new LoggerManager();
    public static LoggerManager Get()
    {
        return instance;
    }

    public long GetTime()
    {
        return System.currentTimeMillis();
    }
    public void SendLog(String message)
    {
        Log.i("myLOG:",message);
    }
}
