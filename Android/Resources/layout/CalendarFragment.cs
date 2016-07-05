using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Android.Resources.layout
{
    public class CalendarFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
            //CaldroidFragment caldroidFragment = new CaldroidFragment();
            //Bundle args = new Bundle();
            //Calendar cal = Calendar.getInstance();
            //args.putInt(CaldroidFragment.MONTH, cal.get(Calendar.MONTH) + 1);
            //args.putInt(CaldroidFragment.YEAR, cal.get(Calendar.YEAR));
            //caldroidFragment.setArguments(args);

            //android.support.v4.app.FragmentTransaction t = getSupportFragmentManager().beginTransaction();
            //t.replace(R.id.cal, caldroidFragment);
            //t.commit();
        }
    }
}