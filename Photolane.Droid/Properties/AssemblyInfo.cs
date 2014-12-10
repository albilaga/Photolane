﻿using System.Reflection;
using System.Runtime.InteropServices;
using Android.App;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Photolane.Droid")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Photolane.Droid")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// This will prevent other apps on the device from receiving GCM messages for this app
// It is crucial that the package name does not start with an uppercase letter - this is forbidden by Android.
[assembly: Permission(Name = "gemastik.photolane.droid.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "gemastik.photolane.droid.permission.C2D_MESSAGE")]

// Gives the app permission to register and receive messages.
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

// This permission is necessary only for Android 4.0.3 and below.
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]

// Need to access the internet for GCM
[assembly: UsesPermission(Name = "android.permission.INTERNET")]

// Needed to keep the processor from sleeping when a message arrives
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]