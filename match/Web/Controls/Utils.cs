using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Main.Models;

namespace Web.Controls
{

    public class Utils
    {

        public Dictionary<int, List<string>> CombineUtils(CombineListModel model,bool IsCheckGroup=true)
        {
            List<string> men = new List<string>();
            List<string> women = new List<string>();
            foreach (CombineModel combine in model.list)
            {
                if (combine.sexy == "1")
                {
                    men.Add(combine.teamid);
                }
                else if (combine.sexy == "2")
                {
                    women.Add(combine.teamid);
                }
            }

            int groupCount = (men.Count() + women.Count()) / 5;

            if (IsCheckGroup)
            {
                if (groupCount > men.Count() || groupCount > women.Count())
                {
                    //  System.out.print("男女比例不符合规则");
                    return null;
                }
                else
                {
                    return partitionGroups(men, women);
                }
            }
            else
            {

                return partitionGroups(men, women);
            }
            
        }
      

        public Dictionary<int, List<string>> partitionGroups(List<string> men, List<string> women)
        {


        int groupCount = (men.Count() + women.Count())/5;
        //if (groupCount > men.Count() || groupCount > women.Count())
        //{
        //  //  System.out.print("男女比例不符合规则");
        //    return null;
        //}
        int maxCount = men.Count() > women.Count() ? men.Count():women.Count();

        Dictionary<int,List<string>> groupMap = new Dictionary<int,List<string>>();
        for (int j = 0; j < groupCount; j ++){
            List<string> group = new List<string>();
            groupMap.Add(j,group);

        }

        for (int i = 0; i < maxCount ; i ++){
            int groupIndex = i % groupCount;
            List<string> group = (List<string>) groupMap[groupIndex];
            if (group.Count() >= 5){
                for (int j = groupIndex; j< groupCount;j ++){
                    group = (List<string>) groupMap[j];
                    if (group.Count() < 5)break;
                }
            }
            if (men.Count() > i){
                if (group.Count < 5)
                {
                    group.Add(men[i]);
                }
            }
            if (group.Count() >= 5){
                for (int j = groupIndex; j< groupCount;j ++){
                    group = (List<string>) groupMap[j];
                    if (group.Count() < 5)break;
                }
            }
            if (women.Count() > i){
                if (group.Count < 5)
                {
                    group.Add(women[i]);
                }
            }
        }

      //  System.out.println(groupMap.toString());
        return groupMap;


    }
    }

}