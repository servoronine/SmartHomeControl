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

public class base_activity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    fragment_light fragmentLight;
    fragment_light_add fragmentLightAdd;
    fragment_heating fragmentHeating;
    fragment_theme fragmentTheme;
    fragment_tv fragmentTv;
    fragment_music fragmentMusic;
    FragmentTransaction fragmentTransaction;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.voronin.smarthomecontrol.R.layout.base_layout);

        fragmentLight = new fragment_light();
        fragmentLightAdd = new fragment_light_add();
        fragmentHeating = new fragment_heating();
        fragmentTheme = new fragment_theme();
        fragmentTv = new fragment_tv();
        fragmentMusic = new fragment_music();

        Toolbar toolbar = (Toolbar) findViewById(com.voronin.smarthomecontrol.R.id.toolbar);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_base);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, com.voronin.smarthomecontrol.R.string.navigation_drawer_open, com.voronin.smarthomecontrol.R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(com.voronin.smarthomecontrol.R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        View layout = (View) findViewById(com.voronin.smarthomecontrol.R.id.layout_base);

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

    public void onMinusButtonClick(View view){
        fragmentTransaction = getFragmentManager().beginTransaction();
        TextView textView = (TextView)findViewById(com.voronin.smarthomecontrol.R.id.heating_degree);
        int heating_degrees = Integer.parseInt((textView.getText()).toString());
        textView.setText(String.valueOf(heating_degrees - 1));
        fragmentTransaction.commit();
    }

    public void onPlusButtonClick(View view){
        fragmentTransaction = getFragmentManager().beginTransaction();
        TextView textView = (TextView)findViewById(com.voronin.smarthomecontrol.R.id.heating_degree);
        int heating_degrees = Integer.parseInt((textView.getText()).toString());
        textView.setText(String.valueOf(heating_degrees + 1));
        fragmentTransaction.commit();
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_base);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
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

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        DrawerLayout drawer = (DrawerLayout) findViewById(com.voronin.smarthomecontrol.R.id.drawer_layout_base);

        switch (id){
            case com.voronin.smarthomecontrol.R.id.login_layout:
                Intent intent0 = new Intent(base_activity.this, login.class);
                startActivity(intent0);
                return true;

            case com.voronin.smarthomecontrol.R.id.main_verst:
                Intent intent1 = new Intent(base_activity.this, main.class);
                startActivity(intent1);
                return true;

            case com.voronin.smarthomecontrol.R.id.light:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentLight);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.light_add:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentLightAdd);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;


            case com.voronin.smarthomecontrol.R.id.heating_verst:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentHeating);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.lighting:
                Intent intent4 = new Intent(base_activity.this, lighting.class);
                startActivity(intent4);
                return true;

            case com.voronin.smarthomecontrol.R.id.theme:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentTheme);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.tv:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentTv);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.energy:
                Intent intent7 = new Intent(base_activity.this, energy.class);
                startActivity(intent7);
                return true;

            case com.voronin.smarthomecontrol.R.id.vacation:
                Intent intent8 = new Intent(base_activity.this, vacation.class);
                startActivity(intent8);
                return true;

            case com.voronin.smarthomecontrol.R.id.music_new:
                fragmentTransaction = getFragmentManager().beginTransaction();
                fragmentTransaction.replace(com.voronin.smarthomecontrol.R.id.fragment_container, fragmentMusic);
                fragmentTransaction.commit();
                drawer.closeDrawer(GravityCompat.START);
                return true;

            case com.voronin.smarthomecontrol.R.id.music_playlist_verst:
                Intent intent9 = new Intent(base_activity.this, music_playlist.class);
                startActivity(intent9);
                return true;
        }

        //DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout_base);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }
}
