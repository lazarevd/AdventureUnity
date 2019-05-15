using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface Tool
    {

        ToolStatus getStatus();

        void setStatus(ToolStatus toolStatus);

        void prepare();

        void select();

        void process();

        void finish();

    }
