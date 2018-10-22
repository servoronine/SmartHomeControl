package com.voronin.smarthomecontrol;

import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.NavUtils;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;

import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.Toast;
import com.github.mikephil.charting.charts.LineChart;
import com.github.mikephil.charting.components.*;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.formatter.AxisValueFormatter;
import com.github.mikephil.charting.utils.ColorTemplate;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.List;

import static java.lang.Integer.parseInt;

/**
 * Created by Михаил on 07.07.2016.
 */
public class energy extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.voronin.smarthomecontrol.R.layout.energy_layout);

        GregorianCalendar gregorianCalendar = new GregorianCalendar();

        Spinner firstDaySpinner = (Spinner) findViewById(com.voronin.smarthomecontrol.R.id.firstDay);
        Spinner firstDateSpinner = (Spinner) findViewById(com.voronin.smarthomecontrol.R.id.firstDate);
        Spinner secondDaySpinner = (Spinner) findViewById(com.voronin.smarthomecontrol.R.id.secondDay);
        Spinner secondDateSpinner = (Spinner) findViewById(com.voronin.smarthomecontrol.R.id.secondDate);

        List<String> firstDates = new ArrayList<String>();
        for (int i = 1; i<= daysInMonth(gregorianCalendar); i++) {
            firstDates.add(String.valueOf(i));
        }

        ArrayAdapter<String> firstDataAdapter = new ArrayAdapter<String>(this, com.voronin.smarthomecontrol.R.layout.spinner_item, firstDates);
        firstDataAdapter.setDropDownViewResource(com.voronin.smarthomecontrol.R.layout.spinner_item);
        firstDaySpinner.setAdapter(firstDataAdapter);
        firstDaySpinner.setOnItemSelectedListener(new CustomOnItemSelectedListener());

        secondDaySpinner.setAdapter(firstDataAdapter);
        secondDaySpinner.setOnItemSelectedListener(new CustomOnItemSelectedListener());

        ArrayAdapter<CharSequence> dateAdapter = ArrayAdapter.createFromResource(this, com.voronin.smarthomecontrol.R.array.month, com.voronin.smarthomecontrol.R.layout.spinner_item);
        dateAdapter.setDropDownViewResource(com.voronin.smarthomecontrol.R.layout.spinner_item);

        firstDateSpinner.setAdapter(dateAdapter);
        firstDateSpinner.setOnItemSelectedListener(new mOnItemSelectedListener());

        secondDateSpinner.setAdapter(dateAdapter);
        secondDateSpinner.setOnItemSelectedListener(new mOnItemSelectedListener());

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

        LineChart chart = (LineChart)findViewById(com.voronin.smarthomecontrol.R.id.chart1);
        chart.setDescription("");
/*        chart.getXAxis().setAxisMaxValue(12);
        chart.getXAxis().setAxisMinValue(0);*/

        final String[] quarters = new String[] { "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb" };

        AxisValueFormatter formatter = new AxisValueFormatter() {

            @Override
            public String getFormattedValue(float value, AxisBase axis) {
                return quarters[(int) value];
            }

            @Override
            public int getDecimalDigits() {  return 0; }
        };

        XAxis xAxis = chart.getXAxis();
        xAxis.setGranularity(2);
        xAxis.setValueFormatter(formatter);
        xAxis.setPosition(XAxis.XAxisPosition.BOTTOM);
        xAxis.setTextSize(13);
        xAxis.setTextColor(Color.WHITE);
        xAxis.setDrawAxisLine(false);
        xAxis.setDrawGridLines(false);

        chart.getAxisLeft().setAxisMaxValue(30);
        chart.getAxisLeft().setAxisMinValue(0);

        YAxis yAxis = chart.getAxisLeft();
        yAxis.setTextSize(13);
        yAxis.setTextColor(Color.WHITE);
        yAxis.setDrawAxisLine(false);
        yAxis.setDrawGridLines(true);
        yAxis.setDrawZeroLine(false);
        yAxis.setGridLineWidth(2);
        yAxis.setGridColor(ColorTemplate.rgb("#dee2e7"));
        yAxis.setGranularity(10);
        yAxis.setLabelCount(4, true);

        YAxis rightAxis = chart.getAxisRight();
        rightAxis.setEnabled(false);

        List<Entry> entries = new ArrayList<Entry>();
        entries.add(new Entry(0, 3));
        entries.add(new Entry(2, 4));
        entries.add(new Entry(4, 10));
        entries.add(new Entry(6, 4));
        entries.add(new Entry(10, 3));

        LineDataSet dataSet = new LineDataSet(entries, "Label");
        dataSet.setMode(dataSet.getMode() == LineDataSet.Mode.CUBIC_BEZIER
                ? LineDataSet.Mode.LINEAR
                : LineDataSet.Mode.LINEAR);
        dataSet.setAxisDependency(YAxis.AxisDependency.LEFT);
        dataSet.setDrawValues(false);
        dataSet.setColor(ColorTemplate.rgb("#d9c26c"));
        dataSet.setLineWidth(3);
        dataSet.setCircleRadius(5);
        dataSet.setCircleColor(ColorTemplate.rgb("#d9c26c"));
        dataSet.setCircleColorHole(ColorTemplate.rgb("#d9c26c"));
        LineData lineData = new LineData(dataSet);
        chart.setData(lineData);
        chart.invalidate();
    }

    public static int daysInMonth(GregorianCalendar c) {
        int [] daysInMonth = {31,28,31,30,31,30,31,31,30,31,30,31};
        daysInMonth[1] += c.isLeapYear(c.get(GregorianCalendar.YEAR)) ? 1 : 0;
        return daysInMonth[c.get(GregorianCalendar.MONTH)];
    }

    public class mOnItemSelectedListener implements AdapterView.OnItemSelectedListener {

        public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
            // On selecting a spinner item
            String item = parent.getItemAtPosition(position).toString();
            // Showing selected spinner item
            Toast.makeText(parent.getContext(), "Selected: " + item, Toast.LENGTH_LONG).show();
        }

        public void onNothingSelected(AdapterView<?> arg0) {
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
