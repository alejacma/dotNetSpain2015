﻿using System;
using SQLite.Net.Interop;
using System.Runtime.InteropServices;
using System.Text;
using Sqlite3DatabaseHandle = System.IntPtr;
using Sqlite3Statement = System.IntPtr;

namespace SQLite.Net.Platform.WinRT
{
    public class SQLiteApiWinRT : ISQLiteApi
    {
        public int BindBlob(IDbStatement stmt, int index, byte[] val, int n, IntPtr free)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindBlob(dbStatement.InternalStmt, index, val, n, free);
        }

        public int BindDouble(IDbStatement stmt, int index, double val)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindDouble(dbStatement.InternalStmt, index, val);
        }

        public int BindInt(IDbStatement stmt, int index, int val)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindInt(dbStatement.InternalStmt, index, val);
        }

        public int BindInt64(IDbStatement stmt, int index, long val)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindInt64(dbStatement.InternalStmt, index, val);
        }

        public int BindNull(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindNull(dbStatement.InternalStmt, index);
        }

        public int BindParameterIndex(IDbStatement stmt, string name)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindParameterIndex(dbStatement.InternalStmt, name);
        }

        public int BindText16(IDbStatement stmt, int index, string val, int n, IntPtr free)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.BindText(dbStatement.InternalStmt, index, val, n, free);
        }

        public Result BusyTimeout(IDbHandle db, int milliseconds)
        {
            var dbHandle = (DbHandle)db;
            return (Result)SQLite3.BusyTimeout(dbHandle.InternalDbHandle, milliseconds);
        }

        public int Changes(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return SQLite3.Changes(dbHandle.InternalDbHandle);
        }

        public Result Close(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return (Result)SQLite3.Close(dbHandle.InternalDbHandle);
        }

        public Result Initialize()
        {
            throw new NotSupportedException();
        }
        public Result Shutdown()
        {
            throw new NotSupportedException();
        }

        public Result Config(ConfigOption option)
        {
            return (Result)SQLite3.Config(option);
        }


        public byte[] ColumnBlob(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            int length = ColumnBytes(stmt, index);
            byte[] result = new byte[length];
            if (length > 0)
                Marshal.Copy(SQLite3.ColumnBlob(dbStatement.InternalStmt, index), result, 0, length);
            return result;
        }

        public byte[] ColumnByteArray(IDbStatement stmt, int index)
        {
            return ColumnBlob(stmt, index);
        }

        public int ColumnBytes(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.ColumnBytes(dbStatement.InternalStmt, index);
        }

        public int ColumnCount(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.ColumnCount(dbStatement.InternalStmt);
        }

        public double ColumnDouble(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.ColumnDouble(dbStatement.InternalStmt, index);
        }

        public int ColumnInt(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.ColumnInt(dbStatement.InternalStmt, index);
        }

        public long ColumnInt64(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.ColumnInt64(dbStatement.InternalStmt, index);
        }

        public string ColumnName16(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return SQLite3.ColumnName16(dbStatement.InternalStmt, index);
        }

        public string ColumnText16(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return Marshal.PtrToStringUni(SQLite3.ColumnText16(dbStatement.InternalStmt, index));
        }

        public ColType ColumnType(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return (ColType)SQLite3.ColumnType(dbStatement.InternalStmt, index);
        }

        public int LibVersionNumber()
        {
            return SQLite3.sqlite3_libversion_number();
        }

        public Result EnableLoadExtension(IDbHandle db, int onoff)
        {
            return (Result)1;
        }

        public string Errmsg16(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return SQLite3.GetErrmsg(dbHandle.InternalDbHandle);
        }

        public Result Finalize(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            Sqlite3Statement internalStmt = dbStatement.InternalStmt;
            return (Result)SQLite3.Finalize(internalStmt);
        }

        public long LastInsertRowid(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return SQLite3.LastInsertRowid(dbHandle.InternalDbHandle);
        }

        public Result Open(byte[] filename, out IDbHandle db, int flags, IntPtr zvfs)
        {
            Sqlite3DatabaseHandle internalDbHandle;
            var ret = (Result)SQLite3.Open(filename, out internalDbHandle, flags, zvfs);
            db = new DbHandle(internalDbHandle);
            return ret;
        }

        public ExtendedResult ExtendedErrCode(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return SQLite3.sqlite3_extended_errcode(dbHandle.InternalDbHandle);
        }

        public IDbStatement Prepare2(IDbHandle db, string query)
        {
            var dbHandle = (DbHandle)db;
            var stmt = default(Sqlite3Statement);
            var r = SQLite3.Prepare2(dbHandle.InternalDbHandle, query, query.Length, out stmt, IntPtr.Zero);
            if (r != Result.OK)
            {
                throw SQLiteException.New(r, SQLite3.GetErrmsg(dbHandle.InternalDbHandle));
            }
            return new DbStatement(stmt);
        }

        public Result Reset(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            return (Result)SQLite3.Reset(dbStatement.InternalStmt);
        }

        public Result Step(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            return (Result)SQLite3.Step(dbStatement.InternalStmt);
        }

        private struct DbHandle : IDbHandle
        {
            public DbHandle(Sqlite3DatabaseHandle internalDbHandle)
                : this()
            {
                InternalDbHandle = internalDbHandle;
            }

            public Sqlite3DatabaseHandle InternalDbHandle { get; set; }

            public bool Equals(IDbHandle other)
            {
                return other is DbHandle && InternalDbHandle == ((DbHandle)other).InternalDbHandle;
            }
        }

        private struct DbStatement : IDbStatement
        {
            public DbStatement(Sqlite3Statement internalStmt)
                : this()
            {
                InternalStmt = internalStmt;
            }

            internal Sqlite3Statement InternalStmt { get; set; }

            public bool Equals(IDbStatement other)
            {
                return (other is DbStatement) && ((DbStatement)other).InternalStmt == InternalStmt;
            }
        }
    }

    public static class SQLite3
    {
        [DllImport("sqlite3", EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open([MarshalAs(UnmanagedType.LPStr)] string filename, out IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open([MarshalAs(UnmanagedType.LPStr)] string filename, out IntPtr db, int flags, IntPtr zvfs);

        [DllImport("sqlite3", EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open(byte[] filename, out IntPtr db, int flags, IntPtr zvfs);

        [DllImport("sqlite3", EntryPoint = "sqlite3_open16", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open16([MarshalAs(UnmanagedType.LPWStr)] string filename, out IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Close(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Config(ConfigOption option);

        [DllImport("sqlite3", EntryPoint = "sqlite3_win32_set_directory", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int SetDirectory(uint directoryType, string directoryPath);

        [DllImport("sqlite3", EntryPoint = "sqlite3_busy_timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result BusyTimeout(IntPtr db, int milliseconds);

        [DllImport("sqlite3", EntryPoint = "sqlite3_changes", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Changes(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Prepare2(IntPtr db, [MarshalAs(UnmanagedType.LPStr)] string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);

        [DllImport("sqlite3", EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Step(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Reset(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Finalize(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_last_insert_rowid", CallingConvention = CallingConvention.Cdecl)]
        public static extern long LastInsertRowid(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_errmsg16", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Errmsg(IntPtr db);

        public static string GetErrmsg(IntPtr db)
        {
            return Marshal.PtrToStringUni(Errmsg(db));
        }

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_parameter_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindParameterIndex(IntPtr stmt, [MarshalAs(UnmanagedType.LPStr)] string name);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_null", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindNull(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindInt(IntPtr stmt, int index, int val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindInt64(IntPtr stmt, int index, long val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindDouble(IntPtr stmt, int index, double val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_text16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int BindText(IntPtr stmt, int index, [MarshalAs(UnmanagedType.LPWStr)] string val, int n, IntPtr free);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_blob", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindBlob(IntPtr stmt, int index, byte[] val, int n, IntPtr free);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnCount(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnName(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_name16", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ColumnName16Internal(IntPtr stmt, int index);
        public static string ColumnName16(IntPtr stmt, int index)
        {
            return Marshal.PtrToStringUni(ColumnName16Internal(stmt, index));
        }

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
        public static extern ColType ColumnType(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnInt(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern long ColumnInt64(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ColumnDouble(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_text", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnText(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_text16", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnText16(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_blob", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnBlob(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_bytes", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnBytes(IntPtr stmt, int index);

        public static string ColumnString(IntPtr stmt, int index)
        {
            return Marshal.PtrToStringUni(SQLite3.ColumnText16(stmt, index));
        }

        public static byte[] ColumnByteArray(IntPtr stmt, int index)
        {
            int length = ColumnBytes(stmt, index);
            byte[] result = new byte[length];
            if (length > 0)
                Marshal.Copy(ColumnBlob(stmt, index), result, 0, length);
            return result;
        }

        [DllImport("sqlite3", EntryPoint = "sqlite3_extended_errcode", CallingConvention = CallingConvention.Cdecl)]
        public static extern ExtendedResult sqlite3_extended_errcode(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_libversion_number", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_libversion_number();
    }
}