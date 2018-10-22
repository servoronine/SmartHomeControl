package com.voronin.smarthomecontrol;

import android.graphics.Color;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Toast;

import java.util.Calendar;

import static java.lang.Integer.parseInt;

/**
 * Created by Михаил on 27.10.2016.
 */
public class CustomOnItemSelectedListener implements AdapterView.OnItemSelectedListener {
    Calendar CurrentDateTime = Calendar.getInstance();
    int CurrentMonth = CurrentDateTime.get(Calendar.MONTH);
    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
        // On selecting a spinner item
        String item = parent.getItemAtPosition(position).toString();
        if (position == CurrentMonth){
            view.setBackgroundColor(Color.parseColor("#d9c26c"));
        } else {view.setBackgroundColor(Color.WHITE);}
        // Showing selected spinner item
        Toast.makeText(parent.getContext(), "Selected: " + item, Toast.LENGTH_LONG).show();
    }

    public void onNothingSelected(AdapterView<?> arg0) {
    }
}
