using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace StrictlyStats
{
    [Activity(Label = "Add Dance", Theme = "@style/AppTheme")]
    public class AddDanceActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        Button btnAddDance;
        Button btnCancelDance;
        Spinner weekNumberSpinner;
        EditText editName;
        EditText editDescription;
        Dance dance = new Dance();
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddDance);

            int danceID = Intent.GetIntExtra("DanceID", 0);
            if (danceID > 0)
            {
                dance = uow.Dances.GetById(danceID);
            }

            editName = FindViewById<EditText>(Resource.Id.editName);
            weekNumberSpinner = FindViewById<Spinner>(Resource.Id.weekNumberSpinner);
            editDescription = FindViewById<EditText>(Resource.Id.editDescription);

            editName.Text = dance.DanceName;
            editDescription.Text = dance.Description;

            int[] items = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, items);
            weekNumberSpinner.Adapter = adapter;

            weekNumberSpinner.SetSelection(Convert.ToInt16(dance.DegreeOfDifficulty) -1);

            btnAddDance = FindViewById<Button>(Resource.Id.btnAddDance);
            btnAddDance.Click += (sender, e) => { BtnSaveDance_Click(); };

            btnCancelDance = FindViewById<Button>(Resource.Id.btnCancelAddDance);
            btnCancelDance.Click += (sender, e) =>  { btnCancelDance_Click(); };
        }
        private void BtnSaveDance_Click()
        {
            dance.DanceName = editName.Text;
            int position = weekNumberSpinner.SelectedItemPosition;
            dance.DegreeOfDifficulty = Convert.ToInt32(weekNumberSpinner.GetItemAtPosition(position));
            dance.Description = editDescription.Text;
            var dlgAlert = (new Android.App.AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage("Please confirm saving the following dance to database: " + dance.DanceName);
            dlgAlert.SetTitle("Save dance?");
            dlgAlert.SetButton("OK", (c, ev) =>
            {
                if (dance.DanceID > 0)
                {
                    uow.Dances.Update(dance);
                } else
                {
                    uow.Dances.Insert(dance);
                }
                Finish();
            });
            dlgAlert.SetButton2("CANCEL", (c, ev) => { });
            dlgAlert.Show();
        }

        private void btnCancelDance_Click()
        {
            Finish();
        }

    }
}