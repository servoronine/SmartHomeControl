package com.voronin.smarthomecontrol;

import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

/**
 * Created by Михаил on 30.08.2016.
 */
public class fragment_main_hot_water extends Fragment {
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        return inflater.inflate(com.voronin.smarthomecontrol.R.layout.main_hot_water, null);
    }
}
