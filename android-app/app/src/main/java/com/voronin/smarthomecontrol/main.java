package com.voronin.smarthomecontrol;

import android.app.FragmentTransaction;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;

import java.util.Calendar;

public class main extends AppCompatActivity
            implements NavigationView.OnNavigationItemSelectedListener {

    fragment_main_tiles fragmentMainTiles;
    fragment_main_onholiday_athome fragmentMainOnholidayAthome;
    fragment_main_temperature fragmentTemperature;
    fragment_main_hot_water fragmentMainHotWater;
    FragmentTransaction fragmentTransaction;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.voronin.smarthomecontrol.R.layout.activity_main);

        fragmentMainTiles = new fragment_main_tiles();
        fragmentMainOnholidayAthome = new fragment_main_onholiday_athome();
        fragmentTemperature = new fragment_main_temperature();
        fragmentMainHotWater = new fragment_main_hot_water();

        Toolbar toolbar = (Toolbar) findViewById(com.voronin.smarthomecontrol.R.id.toolbar);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_main);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, com.voronin.smarthomecontrol.R.string.navigation_drawer_open, com.voronin.smarthomecontrol.R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(com.voronin.smarthomecontrol.R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        View layout = (View) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_main);

        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setDisplayShowHomeEnabled(true);
            getSupportActionBar().setHomeAsUpIndicator(com.voronin.smarthomecontrol.R.drawable.icon_menu_24);
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

    public void onMinusButtonMainClick(View view){
        fragmentTransaction = getFragmentManager().beginTransaction();
        TextView textView = (TextView)findViewById(com.voronin.smarthomecontrol.R.id.main_temperature_degrees);
        int temperature_degrees = Integer.parseInt((textView.getText()).toString());
        textView.setText(String.valueOf(temperature_degrees - 1));
        fragmentTransaction.commit();
    }

    public void onPlusButtonMainClick(View view){
        fragmentTransaction = getFragmentManager().beginTransaction();
        TextView textView = (TextView)findViewById(com.voronin.smarthomecontrol.R.id.main_temperature_degrees);
        int temperature_degrees = Integer.parseInt((textView.getText()).toString());
        textView.setText(String.valueOf(temperature_degrees + 1));
        fragmentTransaction.commit();
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_main);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(com.voronin.smarthomecontrol.R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_main);

        switch (id){
            case com.voronin.smarthomecontrol.R.id.login_layout:
                Intent intent1 = new Intent(main.this, login.class);
                startActivity(intent1);
                return true;

            case com.voronin.smarthomecontrol.R.id.base:
                Intent intent2 = new Intent(main.this, base_activity.class);
                startActivity(intent2);
                return true;

            case com.voronin.smarthomecontrol.R.id.room_new:
                Intent intent3 = new Intent(main.this, new_room.class);
                startActivity(intent3);
                return true;

            case com.voronin.smarthomecontrol.R.id.tiles:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentMainTiles);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case R.id.available_rooms:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, new available_rooms_tiles());
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;
/*            case R.id.onHoliday_AtHome:

                drawer.closeDrawer(GravityCompat.START);
                return  true;*/

            case com.voronin.smarthomecontrol.R.id.temperature:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentTemperature);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.hot_water:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentMainHotWater);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.lighting:
                Intent intent4 = new Intent(main.this, lighting.class);
                startActivity(intent4);
                return true;

            case com.voronin.smarthomecontrol.R.id.energy:
                Intent intent7 = new Intent(main.this, energy.class);
                startActivity(intent7);
                return true;

            case com.voronin.smarthomecontrol.R.id.vacation:
                Intent intent8 = new Intent(main.this, vacation.class);
                startActivity(intent8);
                return true;

            case com.voronin.smarthomecontrol.R.id.music_playlist_verst:
                Intent intent9 = new Intent(main.this, music_playlist.class);
                startActivity(intent9);
                return true;
            case com.voronin.smarthomecontrol.R.id.going_map:
                Intent intent10 = new Intent(main.this, map.class);
                startActivity(intent10);
                return true;
        }

        drawer.closeDrawer(GravityCompat.START);
        return true;
    }
}
