using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

[assembly: Permission(Name = "marvelApp.XamarinForms.Droid.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "marvelApp.XamarinForms.Droid.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace MarvelApp.XamarinForms.Droid.Services
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] { "<<Your Proyect ID>>" };
    }

    /// <summary>
    /// Servicio que registra las notificaciones push con Azure Mobile Services, y las recibe
    /// </summary>
    [Service]
    public class GcmService : GcmServiceBase
    {
        public GcmService()
            : base(GcmBroadcastReceiver.SENDER_IDS)
        {
        }

        protected override async void OnRegistered(Context context, string registrationId)
        {
            Push push = App.ServiceLocator.MarvelBackendDataContext.MobileServiceClient.GetPush();

            try
            {
                await push.UnregisterAllAsync(registrationId);
                await push.RegisterNativeAsync(registrationId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("GcmService.OnRegistered error: {0}", ex.Message));
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            string messageText = intent.Extras.GetString("MarvelApp");
            if (!string.IsNullOrEmpty(messageText))
            {
                CreateNotification("MarvelApp", messageText);
                return;
            }
        }

        private void CreateNotification(string title, string desc)
        {
            // TODO: usar una API que no esté deprecated
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            var uiIntent = new Intent(this, typeof(MainActivity));
            var notification = new Notification(MarvelApp.XamarinForms.Droid.Resource.Drawable.Icon, title);
            notification.Flags = NotificationFlags.AutoCancel;
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));
            notificationManager.Notify(1, notification);
        }

        protected override void OnError(Context context, string errorId)
        {
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
        }
    }
}