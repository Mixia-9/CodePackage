﻿using System;
using System.Runtime.InteropServices;

public static class SQLite3
{
    public enum Result
    {
        OK = 0,
        Error = 1,
        Internal = 2,
        Perm = 3,
        Abort = 4,
        Busy = 5,
        Locked = 6,
        NoMem = 7,
        ReadOnly = 8,
        Interrupt = 9,
        IOError = 10,
        Corrupt = 11,
        NotFound = 12,
        Full = 13,
        CannotOpen = 14,
        LockErr = 15,
        Empty = 16,
        SchemaChngd = 17,
        TooBig = 18,
        Constraint = 19,
        Mismatch = 20,
        Misuse = 21,
        NotImplementedLFS = 22,
        AccessDenied = 23,
        Format = 24,
        Range = 25,
        NonDBFile = 26,
        Notice = 27,
        Warning = 28,
        Row = 100,
        Done = 101
    }

    public enum ExtendedResult
    {
        IOErrorRead = (Result.IOError | (1 << 8)),
        IOErrorShortRead = (Result.IOError | (2 << 8)),
        IOErrorWrite = (Result.IOError | (3 << 8)),
        IOErrorFsync = (Result.IOError | (4 << 8)),
        IOErrorDirFSync = (Result.IOError | (5 << 8)),
        IOErrorTruncate = (Result.IOError | (6 << 8)),
        IOErrorFStat = (Result.IOError | (7 << 8)),
        IOErrorUnlock = (Result.IOError | (8 << 8)),
        IOErrorRdlock = (Result.IOError | (9 << 8)),
        IOErrorDelete = (Result.IOError | (10 << 8)),
        IOErrorBlocked = (Result.IOError | (11 << 8)),
        IOErrorNoMem = (Result.IOError | (12 << 8)),
        IOErrorAccess = (Result.IOError | (13 << 8)),
        IOErrorCheckReservedLock = (Result.IOError | (14 << 8)),
        IOErrorLock = (Result.IOError | (15 << 8)),
        IOErrorClose = (Result.IOError | (16 << 8)),
        IOErrorDirClose = (Result.IOError | (17 << 8)),
        IOErrorSHMOpen = (Result.IOError | (18 << 8)),
        IOErrorSHMSize = (Result.IOError | (19 << 8)),
        IOErrorSHMLock = (Result.IOError | (20 << 8)),
        IOErrorSHMMap = (Result.IOError | (21 << 8)),
        IOErrorSeek = (Result.IOError | (22 << 8)),
        IOErrorDeleteNoEnt = (Result.IOError | (23 << 8)),
        IOErrorMMap = (Result.IOError | (24 << 8)),
        LockedSharedcache = (Result.Locked | (1 << 8)),
        BusyRecovery = (Result.Busy | (1 << 8)),
        CannottOpenNoTempDir = (Result.CannotOpen | (1 << 8)),
        CannotOpenIsDir = (Result.CannotOpen | (2 << 8)),
        CannotOpenFullPath = (Result.CannotOpen | (3 << 8)),
        CorruptVTab = (Result.Corrupt | (1 << 8)),
        ReadonlyRecovery = (Result.ReadOnly | (1 << 8)),
        ReadonlyCannotLock = (Result.ReadOnly | (2 << 8)),
        ReadonlyRollback = (Result.ReadOnly | (3 << 8)),
        AbortRollback = (Result.Abort | (2 << 8)),
        ConstraintCheck = (Result.Constraint | (1 << 8)),
        ConstraintCommitHook = (Result.Constraint | (2 << 8)),
        ConstraintForeignKey = (Result.Constraint | (3 << 8)),
        ConstraintFunction = (Result.Constraint | (4 << 8)),
        ConstraintNotNull = (Result.Constraint | (5 << 8)),
        ConstraintPrimaryKey = (Result.Constraint | (6 << 8)),
        ConstraintTrigger = (Result.Constraint | (7 << 8)),
        ConstraintUnique = (Result.Constraint | (8 << 8)),
        ConstraintVTab = (Result.Constraint | (9 << 8)),
        NoticeRecoverWAL = (Result.Notice | (1 << 8)),
        NoticeRecoverRollback = (Result.Notice | (2 << 8))
    }

#if UNITY_EDITOR_OSX
    private const string DllName = "wxsqlite3";
#elif UNITY_EDITOR_WIN
    private const string DllName = "sqlite3";
#else
    #if UNITY_IOS
    private const string DllName = "__Internal";
    #else
    private const string DllName = "sqlite3";
    #endif
#endif

    [DllImport(DllName, EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result Open(byte[] filename, out IntPtr db, int flags, IntPtr zvfs);

    [DllImport(DllName, EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result Close(IntPtr db);

    [DllImport(DllName, EntryPoint = "sqlite3_busy_timeout", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result BusyTimeout(IntPtr db, int milliseconds);

    [DllImport(DllName, EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result Prepare2(IntPtr db, [MarshalAs(UnmanagedType.LPStr)] string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);



        [DllImport(DllName, EntryPoint = "sqlite3_key", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Key(IntPtr db, [MarshalAs(UnmanagedType.LPStr)] string pKey, int nkey);

        [DllImport(DllName, EntryPoint = "sqlite3_rekey", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result ReKey(IntPtr db, [MarshalAs(UnmanagedType.LPStr)] string pKey, int nkey);

    public static IntPtr Prepare2(IntPtr db, string query)
    {
        IntPtr stmt;
        var r = Prepare2(db, query, System.Text.UTF8Encoding.UTF8.GetByteCount(query), out stmt, IntPtr.Zero);
        if (r != Result.OK)
        {
            //throw SQLiteException.New(r, GetErrmsg(db));
            new Exception("sql error");
        }
        return stmt;
    }

    [DllImport(DllName, EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result Step(IntPtr stmt);


    [DllImport(DllName, EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result Finalize(IntPtr stmt);


    [DllImport(DllName, EntryPoint = "sqlite3_errmsg16", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr Errmsg(IntPtr db);

    public static string GetErrmsg(IntPtr db)
    {
        return Marshal.PtrToStringUni(Errmsg(db));
    }

    [DllImport(DllName, EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ColumnCount(IntPtr stmt);

    [DllImport(DllName, EntryPoint = "sqlite3_column_name16", CallingConvention = CallingConvention.Cdecl)]
    static extern IntPtr ColumnName16Internal(IntPtr stmt, int index);
    public static string ColumnName16(IntPtr stmt, int index)
    {
        return Marshal.PtrToStringUni(ColumnName16Internal(stmt, index));
    }

    [DllImport(DllName, EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern ColType ColumnType(IntPtr stmt, int index);

    [DllImport(DllName, EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ColumnInt(IntPtr stmt, int index);

    [DllImport(DllName, EntryPoint = "sqlite3_column_int64", CallingConvention = CallingConvention.Cdecl)]
    public static extern long ColumnInt64(IntPtr stmt, int index);

    [DllImport(DllName, EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
    public static extern double ColumnDouble(IntPtr stmt, int index);

    [DllImport(DllName, EntryPoint = "sqlite3_column_text16", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ColumnText16(IntPtr stmt, int index);

    [DllImport(DllName, EntryPoint = "sqlite3_column_blob", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ColumnBlob(IntPtr stmt, int index);

    [DllImport(DllName, EntryPoint = "sqlite3_column_bytes", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ColumnBytes(IntPtr stmt, int index);

    public static string ColumnString(IntPtr stmt, int index)
    {
        return Marshal.PtrToStringUni(SQLite3.ColumnText16(stmt, index));
    }

    public static byte[] ColumnByteArray(IntPtr stmt, int index)
    {
        int length = ColumnBytes(stmt, index);
        var result = new byte[length];
        if (length > 0)
            Marshal.Copy(ColumnBlob(stmt, index), result, 0, length);
        return result;
    }

    public enum ColType
    {
        Integer = 1,
        Float = 2,
        Text = 3,
        Blob = 4,
        Null = 5
    }
}