using BlogApp.BlogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.UI
{
    public class UIRepository
    {
        private static volatile UIRepository instance;
        private static object syncRoot = new Object();

        private UIRepository() { }
        public static UIRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new UIRepository();
                    }
                }

                return instance;
            }
        }

        private ClientInfo client;
        public ClientInfo Client
        {
            get { return client; }
            set { client = value; }
        }

        private BlogAppServiceClient localClient;
        public BlogAppServiceClient LocalClient
        {
            get { return localClient; }
            set { localClient = value; }
        }

        private int _modifiyPost;
        public int ModifyPost
        {
            get { return _modifiyPost; }
            set { _modifiyPost = value; }
        }
    }
}