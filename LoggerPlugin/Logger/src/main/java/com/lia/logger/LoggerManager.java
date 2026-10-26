package com.lia.logger;

import android.util.Log;

public class LoggerManager {
    private static LoggerManager instance = null;
    public static LoggerManager Get()
    {
        if (instance != null) return instance;

        instance = new LoggerManager();
    
        return instance;
    }

    public long GetTime()
    {
        return System.currentTimeMillis();
    }
    public void SendLog(String message)
    {
        Log.i("LOG:",message);
    }
}
