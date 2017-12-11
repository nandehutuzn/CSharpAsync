using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet1._0
{
    abstract class Searcher
    {
        private IAsyncResult asyncCall;
        private Timer asyncTimer;

        public abstract IAsyncResult BeginGetResults(int page, int paseSize, out int itemsReturned, AsyncCallback callback, object state);
        public abstract int EndGetResults(out int itemsReturned, IAsyncResult iar);

        public void OnPerformSearch(object sender, EventArgs e)
        {
            int dummy;
            //方法 1
            asyncCall = BeginGetResults(1, 50, out dummy, null, null);

            asyncTimer = new Timer(OnTimerTick, null, 0, 200);

            //BeginGetResults(1, 50, out dummy, Callback, null); //一般用此方式

            //方法2
            WebRequest req = WebRequest.Create("http://www.google.com/#q=weather");
            req.BeginGetResponse(Callback, req);
        }

        private void OnTimerTick(object state)
        {
            if (asyncCall.IsCompleted)  //该方式已被淘汰
            {
                int resultCount;
                try
                {
                    int result = EndGetResults(out resultCount, asyncCall);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                asyncTimer.Dispose();
            }
        }
        
        private void Callback(IAsyncResult iar)
        {
            WebRequest req = iar.AsyncState as WebRequest;
            try
            {
                WebResponse resp = req.EndGetResponse(iar);  //即使不需要返回值，最好也调用此方法，用于清理资源
            }
            catch { }
        }
    }
}
