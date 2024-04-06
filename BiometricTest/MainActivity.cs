using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Tb = AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using AndroidX.Biometric;
using AndroidX.Core.Content;
using Android.Widget;
using Android.Util;

namespace BiometricTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        string tag = "CUSTOM_LOG";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Tb.Toolbar toolbar = FindViewById<Tb.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Button bt = FindViewById
            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += ButOnClick;

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        private void UpdateSecuritySettingsEnabled()
        {

            var biometricManager = BiometricManager.From(this);
            var biometricsAvailable =
                biometricManager.CanAuthenticate(BiometricManager.Authenticators.BiometricStrong) ==
                BiometricManager.BiometricSuccess;
        }



        public void EnableBiometrics()
        {
            //var passwordStorage = new PasswordStorageManager(this);
            var executor = ContextCompat.GetMainExecutor(this);
            var authCallback = new AuthenticationCallback();

            authCallback.Succeeded += async (_, result) =>
            {
                try
                {
                    //var password = await SecureStorageWrapper.GetDatabasePassword();
                    //passwordStorage.Store(password, result.CryptoObject.Cipher);
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, "error after succ" + e.ToString(), ToastLength.Short).Show();

                    Log.Error(tag,"error after succeding", e);
                    //Logger.Error(e);
                    //callback(false);
                    return;
                }

                //callback(true);
            };

            authCallback.Failed += delegate
            {
                Toast.MakeText(this, "failed to recognize ", ToastLength.Short).Show();

                Log.Error(tag, "failed to recognize ");
                //callback(false);
            };

            authCallback.Errored += (sdf, sf) =>
            {
                Toast.MakeText(this, "error", ToastLength.Short).Show();

                Log.Error(tag, "errrr");
                // Do something, probably
                //callback(false);
            };

            var prompt = new BiometricPrompt(this, executor, authCallback);

            var promptInfo = new BiometricPrompt.PromptInfo.Builder()
                .SetTitle("Title")
                .SetNegativeButtonText("Cancel")
                .SetConfirmationRequired(false)
                .SetAllowedAuthenticators(BiometricManager.Authenticators.BiometricStrong)
                .Build();

       

            //prompt.Authenticate(promptInfo, new BiometricPrompt.CryptoObject(cipher));
            prompt.Authenticate(promptInfo);
        }

        public void ClearBiometrics()
        {
            //var storage = new PasswordStorageManager(this);
            //storage.Clear();
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void ButOnClick(object sender, EventArgs eventArgs)
        {
            EnableBiometrics();
            Toast.MakeText(this, "clicked", ToastLength.Short).Show();
        }


        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
