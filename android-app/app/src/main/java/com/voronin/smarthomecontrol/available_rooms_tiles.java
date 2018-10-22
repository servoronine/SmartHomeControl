package com.voronin.smarthomecontrol;

import android.app.ActionBar;
import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TableLayout;
import android.widget.TableRow;


public class available_rooms_tiles extends Fragment {
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.available_rooms_tiles, null);
        TableLayout tl = (TableLayout)view.findViewById(R.id.available_rooms_tiles_tl);

        TableRow tr = new TableRow(getActivity());
        TableLayout.LayoutParams lp = new TableLayout.LayoutParams();
        lp.width = TableLayout.LayoutParams.MATCH_PARENT;
        lp.height = TableLayout.LayoutParams.MATCH_PARENT;
        tl.addView(tr, lp);

        RelativeLayout rl = new RelativeLayout(getActivity());
        TableRow.LayoutParams lp1 = new TableRow.LayoutParams(TableRow.LayoutParams.MATCH_PARENT,
                TableRow.LayoutParams.MATCH_PARENT);
        rl.setBackgroundResource(R.drawable.button_background);
        rl.setPadding(0, ConvertDpIntoPx(5), 0, ConvertDpIntoPx(7));
        rl.setClickable(true);
        rl.setFocusable(true);
        tr.addView(rl, lp1);

        LinearLayout ll = new LinearLayout(getActivity());
        ll.setOrientation(LinearLayout.HORIZONTAL);
        RelativeLayout.LayoutParams lp2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WRAP_CONTENT,
                RelativeLayout.LayoutParams.WRAP_CONTENT);
        lp2.addRule(RelativeLayout.ALIGN_PARENT_TOP);
        lp2.addRule(RelativeLayout.CENTER_HORIZONTAL);
        rl.addView(ll, lp2);

        ImageView iv = new ImageView(getActivity());
        LinearLayout.LayoutParams lp3 = new LinearLayout.LayoutParams(ConvertDpIntoPx(22),
                LinearLayout.LayoutParams.WRAP_CONTENT);
        iv.setImageResource(R.mipmap.mini_icon_tv);
        ll.addView(iv, lp3);

        return view;
    }

    private int ConvertDpIntoPx(int dp) {
        final float scale = getResources().getDisplayMetrics().density;
        return  (int) (dp * scale + 0.5f);
    }

}
