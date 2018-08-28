using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

// Authors: Lucas Pimenta
// Pipefy x Uipath Integration
// Created on 10th May 2018
// This code is free. Check LICENSE.txt for MIT Open Source License terms.

namespace Pipefy.Organization
{
    public class Delete_Organization : CodeActivity
    {
        [Category("Input")]
        [Description("The user API Key")]
        [RequiredArgument]
        public InArgument<string> Bearer { get; set; }

        [Category("Input")]
        [Description("Organization ID that will be deleted")]
        [RequiredArgument]
        public InArgument<string> OrgID { get; set; }

        [Category("Output")]
        public OutArgument<string> Return { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("https://app.pipefy.com/queries");
            req.Proxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            req.Timeout = 120000;
            req.Headers.Add("authorization", "Bearer " + Bearer.Get(context));
            req.ContentType = "application/json";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("{ \"query\": \"mutation { deleteOrganization(input: {id: " + OrgID.Get(context) + "}) { success } }\" }");
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            JArray array = new JArray();
            JObject joResponse = JObject.Parse(sr.ReadToEnd());
            Return.Set(context, joResponse.ToString());
        }
    }
}
