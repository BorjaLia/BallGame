package com.lia.logger;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.Log;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class LoggerManager {
    private static final LoggerManager instance = new LoggerManager();

    private Activity currentActivity;
    private File logFile;

    public static LoggerManager Get() {
        return instance;
    }

    public void Init(Activity activity) {
        this.currentActivity = activity;
        this.logFile = new File(activity.getExternalFilesDir(null), "unity_logs.txt");
    }

    public long GetTime() {
        return System.currentTimeMillis();
    }

    public void SendLog(String message) {
        Log.i("myLOG:", message);
        if (logFile == null) return;

        try (FileWriter writer = new FileWriter(logFile, true)) {
            writer.append(message).append("\n");
        } catch (IOException e) {
            Log.e("myLOG:", "Error al guardar el log", e);
        }
    }

    public String GetAllLogs() {
        if (logFile == null || !logFile.exists()) {
            return "No hay logs guardados todavia.";
        }

        StringBuilder sb = new StringBuilder();
        try (BufferedReader reader = new BufferedReader(new FileReader(logFile))) {
            String line;
            while ((line = reader.readLine()) != null) {
                sb.append(line).append("\n");
            }
        } catch (IOException e) {
            Log.e("myLOG:", "Error al leer los logs", e);
            return "Error al leer el archivo de logs.";
        }
        return sb.toString();
    }

    public void RequestClearLogs() {
        if (currentActivity == null) return;

        currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                new AlertDialog.Builder(currentActivity)
                        .setTitle("Borrar Registros")
                        .setMessage("¿Estás seguro de que deseas eliminar el archivo de logs?")
                        .setPositiveButton("Si, borrar", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                ClearLogsFile();
                            }
                        })
                        .setNegativeButton("Cancelar", null)
                        .show();
            }
        });
    }

    private void ClearLogsFile() {
        if (logFile != null && logFile.exists()) {
            boolean deleted = logFile.delete();
            if(deleted) {
                Log.i("myLOG:", "Archivo de logs eliminado correctamente.");
            }
        }
    }
}