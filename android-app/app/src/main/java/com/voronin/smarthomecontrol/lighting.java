package com.voronin.smarthomecontrol;

import android.os.Bundle;
import android.support.v4.app.NavUtils;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;
import com.triggertrap.seekarc.SeekArc;

import java.util.Calendar;

/**
 * Created by Михаил on 06.07.2016.
 */
public class lighting extends AppCompatActivity {

    private SeekArc SeekArc;
    private TextView SeekArcProgress;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.voronin.smarthomecontrol.R.layout.lighting_layout);

        SeekArc = (SeekArc) findViewById(com.voronin.smarthomecontrol.R.id.seekArc);
        SeekArcProgress = (TextView)findViewById(com.voronin.smarthomecontrol.R.id.seekArcProgress);

        SeekArc.setOnSeekArcChangeListener(new com.triggertrap.seekarc.SeekArc.OnSeekArcChangeListener() {
            @Override
            public void onStopTrackingTouch(SeekArc seekArc) {
            }
            @Override
            public void onStartTrackingTouch(SeekArc seekArc) {
            }
            @Override
            public void onProgressChanged(SeekArc seekArc, int progress,boolean fromUser)
            {
                SeekArcProgress.setText(String.valueOf(progress));
            }
        });

        Toolbar toolbar = (Toolbar) findViewById(com.voronin.smarthomecontrol.R.id.toolbar);
        setSupportActionBar(toolbar);

        View layout = (View) findViewById(com.voronin.smarthomecontrol.R.id.lighting_layout);

        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setDisplayShowHomeEnabled(true);
            getSupportActionBar().setHomeAsUpIndicator(com.voronin.smarthomecontrol.R.drawable.icon_left_24);
        }

        Calendar CurrentDateTime = Calendar.getInstance();
        int CurrentHour = CurrentDateTime.get(Calendar.HOUR);

        if ((CurrentHour >= 0) & (CurrentHour < 6)) {
            layout.setBackgroundDrawable(getResources().getDrawable(com.voronin.smarthomecontrol.R.drawable.bg_night));
        } else if ((CurrentHour >= 6) & (CurrentHour < 12)) {
            layout.setBackgroundDrawable(getResources().getDrawable(com.voronin.smarthomecontrol.R.drawable.bg_morning));
        } else if ((CurrentHour >= 12) & (CurrentHour < 18)) {
            layout.setBackgroundDrawable(getResources().getDrawable(com.voronin.smarthomecontrol.R.drawable.bg_day));
        } else {
            layout.setBackgroundDrawable(getResources().getDrawable(com.voronin.smarthomecontrol.R.drawable.bg_evening));
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(com.voronin.smarthomecontrol.R.menu.menu_with_folder, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        //noinspection SimplifiableIfStatement

        switch (id){
            case android.R.id.home:
                NavUtils.navigateUpFromSameTask(this);
                return true;

        }

        return super.onOptionsItemSelected(item);
    }
}
