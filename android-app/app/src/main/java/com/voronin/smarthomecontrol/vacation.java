package com.voronin.smarthomecontrol;

import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.NavUtils;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.*;
import com.codetroopers.betterpickers.datepicker.DatePickerBuilder;
import com.codetroopers.betterpickers.datepicker.DatePickerDialogFragment;
import com.codetroopers.betterpickers.timepicker.TimePickerBuilder;
import com.codetroopers.betterpickers.timepicker.TimePickerDialogFragment;

import java.text.SimpleDateFormat;
import java.util.*;

import static java.lang.Integer.parseInt;

/**
 * Created by Михаил on 07.07.2016.
 */
public class vacation extends AppCompatActivity {

    static int hour, min;

    Button fromtimepicker, fromdatepicker, totimepicker, todatepicker;

    java.sql.Time timeValue;
    SimpleDateFormat format;
    Calendar c;
    int year, month, day;
    SimpleDateFormat formatter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.voronin.smarthomecontrol.R.layout.vacation_layout);

        c = Calendar.getInstance();
        hour = c.get(Calendar.HOUR_OF_DAY);
        min = c.get(Calendar.MINUTE);

        year = c.get(Calendar.YEAR);
        month = c.get(Calendar.MONTH);
        day = c.get(Calendar.DAY_OF_MONTH);

        fromdatepicker = (Button) findViewById(com.voronin.smarthomecontrol.R.id.fromdatepicker);
        fromtimepicker = (Button) findViewById(com.voronin.smarthomecontrol.R.id.fromtimepicker);
        todatepicker = (Button) findViewById(com.voronin.smarthomecontrol.R.id.todatepicker);
        totimepicker = (Button) findViewById(com.voronin.smarthomecontrol.R.id.totimepicker);

        Toolbar toolbar = (Toolbar) findViewById(com.voronin.smarthomecontrol.R.id.toolbar);
        setSupportActionBar(toolbar);

        View layout = (View) findViewById(com.voronin.smarthomecontrol.R.id.energy_layout);

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

        fromdatepicker.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DatePickerBuilder dpb = new DatePickerBuilder()
                        .setFragmentManager(getSupportFragmentManager())
                        .setStyleResId(com.voronin.smarthomecontrol.R.style.BetterPickersDialogFragment_Light)
                        .setYearOptional(true);

                dpb.show();
                dpb.addDatePickerDialogHandler(new DatePickerDialogFragment.DatePickerDialogHandler() {
                                                   @Override
                                                   public void onDialogDateSet(int reference, int year, int monthOfYear, int dayOfMonth) {
                                                       try {
                                                           formatter = new SimpleDateFormat("dd/MM/yyyy");
                                                           String dateInString = dayOfMonth + "/" + (monthOfYear + 1) + "/" + year;
                                                           Date date = formatter.parse(dateInString);
                                                           fromdatepicker.setText(formatter.format(date).toString());

                                                           if ((dayOfMonth == c.get(Calendar.DAY_OF_MONTH)) && (monthOfYear == c.get(Calendar.MONTH)) && (year == c.get(Calendar.YEAR))) {
                                                               fromdatepicker.setTextColor(Color.parseColor("#d9c26c"));
                                                           }
                                                       } catch (Exception ex) {
                                                           fromdatepicker.setText(ex.getMessage().toString());
                                                       }
                                                   }
                                               }

                );
            }
        });

        todatepicker.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DatePickerBuilder dpb = new DatePickerBuilder()
                        .setFragmentManager(getSupportFragmentManager())
                        .setStyleResId(com.voronin.smarthomecontrol.R.style.BetterPickersDialogFragment_Light)
                        .setYearOptional(true);

                dpb.show();
                dpb.addDatePickerDialogHandler(new DatePickerDialogFragment.DatePickerDialogHandler() {
                                                   @Override
                                                   public void onDialogDateSet(int reference, int year, int monthOfYear, int dayOfMonth) {
                                                       try {
                                                           formatter = new SimpleDateFormat("dd/MM/yyyy");
                                                           String dateInString = dayOfMonth + "/" + (monthOfYear + 1) + "/" + year;
                                                           Date date = formatter.parse(dateInString);
                                                           todatepicker.setText(formatter.format(date).toString());

                                                           if ((dayOfMonth == c.get(Calendar.DAY_OF_MONTH)) && (monthOfYear == c.get(Calendar.MONTH)) && (year == c.get(Calendar.YEAR))) {
                                                               todatepicker.setTextColor(Color.parseColor("#d9c26c"));
                                                           }
                                                       } catch (Exception ex) {
                                                           todatepicker.setText(ex.getMessage().toString());
                                                       }
                                                   }
                                               }

                );
            }
        });

        fromtimepicker.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                TimePickerBuilder tpb = new TimePickerBuilder()
                        .setFragmentManager(getSupportFragmentManager())
                        .setStyleResId(com.voronin.smarthomecontrol.R.style.BetterPickersDialogFragment_Light);
                tpb.show();
                tpb.addTimePickerDialogHandler(new TimePickerDialogFragment.TimePickerDialogHandler() {
                    @Override
                    public void onDialogTimeSet(int reference, int hourOfDay, int minute) {
                        try {
                            String dtStart = String.valueOf(hourOfDay) + ":" + String.valueOf(minute);
                            format = new SimpleDateFormat("HH:mm");

                            timeValue = new java.sql.Time(format.parse(dtStart).getTime());
                            fromtimepicker.setText(String.valueOf(timeValue));

                            if ((hourOfDay == c.get(Calendar.HOUR_OF_DAY)) && (minute == c.get(Calendar.MINUTE))) {
                                fromtimepicker.setTextColor(Color.parseColor("#d9c26c"));
                            }
                        } catch (Exception ex) {
                            fromtimepicker.setText(ex.getMessage().toString());
                        }
                    }
                });
            }
        });

        totimepicker.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                TimePickerBuilder tpb = new TimePickerBuilder()
                        .setFragmentManager(getSupportFragmentManager())
                        .setStyleResId(com.voronin.smarthomecontrol.R.style.BetterPickersDialogFragment_Light);
                tpb.show();
                tpb.addTimePickerDialogHandler(new TimePickerDialogFragment.TimePickerDialogHandler() {
                    @Override
                    public void onDialogTimeSet(int reference, int hourOfDay, int minute) {
                        try {
                            String dtStart = String.valueOf(hourOfDay) + ":" + String.valueOf(minute);
                            format = new SimpleDateFormat("HH:mm");

                            timeValue = new java.sql.Time(format.parse(dtStart).getTime());
                            totimepicker.setText(String.valueOf(timeValue));

                            if ((hourOfDay == c.get(Calendar.HOUR_OF_DAY)) && (minute == c.get(Calendar.MINUTE))) {
                                totimepicker.setTextColor(Color.parseColor("#d9c26c"));
                            }
                        } catch (Exception ex) {
                            totimepicker.setText(ex.getMessage().toString());
                        }
                    }
                });
            }
        });
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
