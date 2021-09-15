using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMetricaManager : MonoBehaviour
{
    public static AppMetricaManager instance;

    [HideInInspector] public DateTime date;
    [HideInInspector] public DateTime dateStage;

    private void Awake()
    {
        instance = this;
    }

    #region AppMetrica
    public enum Reports { LEVEL_START, LEVEL_FINISH, RATE, STAGE_START, STAGE_FINISH, TUTORIAL, LEVEL_UP, VIDEO_AVAILABLE, VIDEO_STARTED, VIDEO_WATCH }

    public void SendReportAppMetrica(Reports reports)
    {
        string message = null;
        switch (reports)
        {
            case Reports.LEVEL_START:
                message = "level_start";
                break;
            case Reports.LEVEL_FINISH:
                message = "level_finish";
                break;
            case Reports.RATE:
                message = "rate_us";
                break;
        }
        AppMetrica.Instance.ReportEvent(message);
        AppMetrica.Instance.SendEventsBuffer();
    }

    public void SendReportAppMetrica(Reports reports, Dictionary<string, object> vals)
    {
        string message = null;
        switch (reports)
        {
            case Reports.LEVEL_START:
                message = "level_start";
                break;
            case Reports.LEVEL_FINISH:
                message = "level_finish";
                break;
            case Reports.RATE:
                message = "rate_us";
                break;
            case Reports.STAGE_START:
                message = "stage_start";
                break;
            case Reports.STAGE_FINISH:
                message = "stage_finish";
                break;
            case Reports.TUTORIAL:
                message = "tutorial";
                break;
            case Reports.LEVEL_UP:
                message = "level_up";
                break;
            case Reports.VIDEO_AVAILABLE:
                message = "video_ads_available";
                break;
            case Reports.VIDEO_STARTED:
                message = "video_ads_started";
                break;
            case Reports.VIDEO_WATCH:
                message = "video_ads_watch";
                break;
        }
        AppMetrica.Instance.ReportEvent(message, vals);
        AppMetrica.Instance.SendEventsBuffer();
    }


    #endregion

    private void Start()
    {
        instance = this;
    }

    #region AppMetrica
    public void LevelStart(int level_number, string level_name, int level_count, string level_diff, int level_loop, bool level_random, string level_type, string game_mode)
    {
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "level_number", level_number},
            { "level_name",  level_name},
            { "level_count", level_count},
            { "level_diff", level_diff},
            { "level_loop", level_loop },
            { "level_random", level_random },
            { "level_type", level_type },
            { "game_mode", game_mode }
        };
        date = DateTime.Now;
        SendReportAppMetrica(Reports.LEVEL_START, vals);
    }

    public void LevelFinish(int level_number, string level_name, int level_count, string level_diff, int level_loop, bool level_random, string level_type, string game_mode, string result, DateTime time, int progress, int _continue)
    {
        TimeSpan ts = time - date;
        int seconds = (int)ts.TotalSeconds;
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "level_number", level_number},
            { "level_name",  level_name},
            { "level_count", level_count},
            { "level_diff", level_diff},
            { "level_loop", level_loop },
            { "level_random", level_random },
            { "level_type", level_type },
            { "game_mode", game_mode },
            { "result", result },
            { "time", seconds },
            { "progress", progress },
            { "continue", _continue },
        };

        SendReportAppMetrica(Reports.LEVEL_FINISH, vals);
    }

    public void LevelFinish(int level_number, string level_name, int level_count, string level_diff, int level_loop, bool level_random, string level_type, string game_mode, string result, int time, int progress, int _continue)
    {

        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "level_number", level_number},
            { "level_name",  level_name},
            { "level_count", level_count},
            { "level_diff", level_diff},
            { "level_loop", level_loop },
            { "level_random", level_random },
            { "level_type", level_type },
            { "game_mode", game_mode },
            { "result", result },
            { "time", time },
            { "progress", progress },
            { "continue", _continue },
        };

        SendReportAppMetrica(Reports.LEVEL_FINISH, vals);
    }

    public void Stage(Reports stage, int stage_number, string stage_name, int stage_count, DateTime time, string result = null,  int progress = 0, int _continue = 0)
    {
        Dictionary<string, object> vals = new Dictionary<string, object>();

        switch (stage)
        {
            case Reports.STAGE_START:
                dateStage = time;
                vals.Add("stage_number", stage_number);
                vals.Add("stage_name", stage_name);
                vals.Add("stage_count", stage_count);
                break;
            case Reports.STAGE_FINISH:
                TimeSpan ts = time - dateStage;
                int seconds = (int)ts.TotalSeconds;
                vals.Add("stage_number", stage_number);
                vals.Add("stage_name", stage_name);
                vals.Add("stage_count", stage_count);
                vals.Add("result", result);
                vals.Add("time", time);
                vals.Add("progress", progress);
                vals.Add("continue", _continue);

                break;
        }
        SendReportAppMetrica(stage, vals);

    }

    public void Rate(string show_reason, int rate_result)
    {
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            {
                "show_reason", show_reason },
            { "rate_result", rate_result }
            };
        SendReportAppMetrica(Reports.RATE, vals);
    }

    public void Tutorial(string step_name)
    {
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            {
                "step_name", step_name },
            };
        SendReportAppMetrica(Reports.TUTORIAL, vals);
    }

    public void LevelUp(int level)
    {
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            {
                "level", level },
            };
        SendReportAppMetrica(Reports.LEVEL_UP, vals);
    }

    public void VideoAds(Reports video, string ad_type, string placement, string result, bool connection)
    {
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", ad_type},
            { "placement",  placement},
            { "result", result},
            { "connection", connection},
        };
        switch (video)
        {
            case Reports.VIDEO_AVAILABLE:
                break;
            case Reports.VIDEO_STARTED:
                break;
            case Reports.VIDEO_WATCH:
                break;
        }

        SendReportAppMetrica(video, vals);
    }
    #endregion
}
