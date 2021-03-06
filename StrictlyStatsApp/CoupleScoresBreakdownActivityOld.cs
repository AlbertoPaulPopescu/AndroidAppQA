using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    //[Activity(Label = "CoupleScoresBreakdown")]
    [Activity(Label = "Couple scores breakdown", Theme = "@style/AppTheme")]
    public class CoupleScoresBreakdownActivityOld : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Couple> couples;
        int _select = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CouplesScoresBreakdown);

            if (savedInstanceState != null)
            {
               _select = savedInstanceState.GetInt("click_count", 0);
            }
            ListView lstVwCouples = FindViewById<ListView>(Resource.Id.lstVwCouples);
            lstVwCouples.ChoiceMode = ChoiceMode.Single;

            couples = uow.Couples.GetAll();

            lstVwCouples.Adapter = new CoupleScoresBreakdownAdapter(this, couples);
            lstVwCouples.ItemClick += LstVwCouples_ItemClick;
            lstVwCouples.SetSelection(_select);
            LstVwCouples_ItemClick(null, new AdapterView.ItemClickEventArgs(null, null, 0, 0));
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("click_count", _select);
            base.OnSaveInstanceState(outState);
        }
        private void LstVwCouples_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView avgTextView = FindViewById<TextView>(Resource.Id.avgTextView);
            IList<Score> scores = uow.Scores.GetScoresForCoupleWithDance(couples[e.Position].CoupleID);
            ListView lstVwCoupleScores = FindViewById<ListView>(Resource.Id.lstVwCoupleScores);
            _select = e.Position;
            lstVwCoupleScores.Adapter = new CoupleScoresBreakdownDetailsAdapter(this, scores);

            Couple couple = uow.Couples.GetById(couples[e.Position].CoupleID);
            avgTextView.Text = ($"Score average: {((Decimal)scores.Sum<Score>(s => s.Grade) / scores.Count):0.00}");

        }

    }
}