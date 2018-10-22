package com.voronin.smarthomecontrol;

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

import java.util.Calendar;

public class login extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.voronin.smarthomecontrol.R.layout.activity_login);
        Toolbar toolbar = (Toolbar) findViewById(com.voronin.smarthomecontrol.R.id.toolbar);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_login);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, com.voronin.smarthomecontrol.R.string.navigation_drawer_open, com.voronin.smarthomecontrol.R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(com.voronin.smarthomecontrol.R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        View layout = (View) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_login);

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

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_login);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(com.voronin.smarthomecontrol.R.menu.menu_basic, menu);
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

        switch (id){
            case com.voronin.smarthomecontrol.R.id.main_verst:
                Intent intent1 = new Intent(login.this, main.class);
                startActivity(intent1);
                return true;

            case com.voronin.smarthomecontrol.R.id.base:
                Intent intent2 = new Intent(login.this, base_activity.class);
                startActivity(intent2);
                return true;

            case com.voronin.smarthomecontrol.R.id.lighting:
                Intent intent4 = new Intent(login.this, lighting.class);
                startActivity(intent4);
                return true;

            case com.voronin.smarthomecontrol.R.id.energy:
                Intent intent7 = new Intent(login.this, energy.class);
                startActivity(intent7);
                return true;

            case com.voronin.smarthomecontrol.R.id.vacation:
                Intent intent8 = new Intent(login.this, vacation.class);
                startActivity(intent8);
                return true;

            case com.voronin.smarthomecontrol.R.id.music_playlist_verst:
                Intent intent9 = new Intent(login.this, music_playlist.class);
                startActivity(intent9);
                return true;
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_login);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

}
