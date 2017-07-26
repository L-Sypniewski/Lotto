// Puts HTML code of each draw into a List. Regex against these HTML codes is executed later.
/* private void AddSeparateValuesToList(List<string> listSeparatedDraws, int iterateFrom, int iterateTo, HtmlNodeCollection nodesSeparateDraws, HtmlNodeCollection nodesPluses = null)
 {
     if (nodesPluses == null)
     {
         for (int i = iterateFrom; i < iterateTo; i++)
         {
             // Plus can be null
             listSeparatedDraws.Add(string.Format("{0} plus:", nodesSeparateDraws[i].InnerText));
         }
     }
     else
     {
         for (int i = iterateFrom; i < iterateTo; i++)
         {
             listSeparatedDraws.Add(string.Format("{0} plus:{1}", nodesSeparateDraws[i].InnerText, nodesPluses[i].InnerText));
         }
     }
 }

 protected virtual void AddValuesToList(List<string> listSeparateDraws, HtmlNodeCollection nodesSeparateDraws, HtmlNodeCollection nodesPluses)
 {
     int HowManyIterationsWithPlus = Int32.Parse(new Regex(@"(?<NrLosowania>\d{1,5})\.").Match(nodesSeparateDraws[0].InnerText).Groups[1].Value) - INDEX_OF_FIRST_DRAW_WITH_PLUS + 1;
     AddSeparateValuesToList(listSeparateDraws, 0, HowManyIterationsWithPlus, nodesSeparateDraws, nodesPluses);
     AddSeparateValuesToList(listSeparateDraws, HowManyIterationsWithPlus, nodesSeparateDraws.Count, nodesSeparateDraws);
 }/*

 



/* PROTOTYPE   Puts HTML code of each draw into a List. Regex against these HTML codes is executed later.
private static void AddValuesToList(List<string> listSeparateDraws, HtmlNodeCollection nodesSeparateDraws, HtmlNodeCollection nodesPluses)
{
    if (nodesPluses == null)
    {
        for (int i = 0; i < nodesSeparateDraws.Count; i++)
        {
            listSeparateDraws.Add(string.Format("{0} plus:", nodesSeparateDraws[i].InnerText));
        }
    }
    else
    {
        // Processing nodes with pluses, since the newest draws are located at the beginning of a nodes collection
        for (int i = 0; i < nodesPluses.Count - 1; i++) // TODO: Investigate why there is unexpected result at a draw number 4346 when there's no '-1' in loop counter
        {
            listSeparateDraws.Add(string.Format("{0} plus:{1}", nodesSeparateDraws[i].InnerText, nodesPluses[i].InnerText));
        }

        for (int i = nodesPluses.Count - 1; i < nodesSeparateDraws.Count - 1; i++)
        {
            listSeparateDraws.Add(string.Format("{0} plus:", nodesSeparateDraws[i].InnerText));
        }
    }
}*/