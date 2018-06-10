﻿using System;
using System.IO;
using Agenda;
using Agenda.Windows;
using SQLite;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]

namespace Agenda.Windows
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
			var documentsPath = ApplicationData.Current.LocalFolder.Path;
        	var path = Path.Combine(documentsPath, "MySQLite.db3");
        	return new SQLiteAsyncConnection(path);
        }
    }
}

