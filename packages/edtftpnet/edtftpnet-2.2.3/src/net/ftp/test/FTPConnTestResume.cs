// edtFTPnet
// 
// Copyright (C) 2004 Enterprise Distributed Technologies Ltd
// 
// www.enterprisedt.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// Bug fixes, suggestions and comments should posted on 
// http://www.enterprisedt.com/forums/index.php
// 
// Change Log:
// 
// $Log: FTPConnTestResume.cs,v $
// Revision 1.4  2012-01-31 00:11:49  bruceb
// TestResumeDownload
//
// Revision 1.3  2008-06-17 06:12:13  bruceb
// net cf changes
//
// Revision 1.2  2007-11-22 01:31:48  bruceb
// FTPTransferCancelledException
//
// Revision 1.1  2006/05/03 04:41:02  bruceb
// FTPConnection tests
//
// Revision 1.3  2005/08/04 21:55:46  bruceb
// changes for re-jigged test subdirs
//
// Revision 1.2  2004/11/20 22:38:22  bruceb
//

using System;
using System.IO;
using NUnit.Framework;
using EnterpriseDT.Net.Ftp;

namespace EnterpriseDT.Net.Ftp.Test
{
	/// <summary>  
	/// Test resuming of binary transfers when interrupted
	/// </summary>
	/// <author>      Bruce Blackshaw
	/// </author>
	/// <version>     $Revision: 1.4 $
	/// </version>
    [TestFixture]
    [Category("edtFTPnet")]
	public class FTPConnTestResume:FTPConnTestCase
	{
		/// <summary>  
		/// Get name of log file
		/// </summary>
		override internal string LogName
		{
			get
			{
				return "FTPConnTestResume.log";
            }
		}
        
		/// <summary> 
        /// Flag used to determine if a transfer should be cancelled
        /// </summary>
        private bool cancelled = false;
        
        
		/// <summary> 
        /// Logs count of bytes transferred via event
        /// </summary>
	    internal void CancelBytesTransferred(object obj, BytesTransferredEventArgs args) 
	    {
            if (!cancelled) 
            {
                log.Debug("BytesTransferred: cancelling transfer");
                ftp.CancelTransfer();
                cancelled = true;
            }
	    }

        /// <summary>Test resuming when putting a binary file</summary>
        [Test]
		public virtual void ResumePut()
		{
			log.Debug("ResumePut()");
			
            try 
            {
    			Connect();
    			
    			// move to test directory
    			ftp.ChangeWorkingDirectory(testdir);
    			ftp.TransferType = FTPTransferType.BINARY;
    			
    			// put to a random filename
    			string filename = GenerateRandomFilename();
                
                // set up event handler to cancel the transfer
                cancelled = false;
                BytesTransferredHandler handler = new BytesTransferredHandler(CancelBytesTransferred);
    			ftp.BytesTransferred += handler;
                
    			// initiate the put
                try
                {
                    ftp.UploadFile(localDataDir + localBinaryFile, filename);
                    Assert.Fail("Should receive cancelled exception");
                }
                catch (FTPTransferCancelledException)
                {
                    log.Debug("Caught expected exception from cancelled transfer");
                }
                
                // remove handler now we've cancelled
    			ftp.BytesTransferred -= handler;
                
    			// should be cancelled - now resume
    			ftp.ResumeTransfer();
    			
    			// put again - should append
    			ftp.UploadFile(localDataDir + localBinaryFile, filename);
    			
    			// get it back        
    			ftp.DownloadFile(localDataDir + filename, filename);
    			
    			// check equality of local files
    			AssertIdentical(localDataDir + localBinaryFile, localDataDir + filename);
    			
    			// finally, just check cancelResume works
    			ftp.CancelResume();
    			
    			// get it back        
    			ftp.DownloadFile(localDataDir + filename, filename);
    			
    			// delete remote file
    			ftp.DeleteFile(filename);
    			
    			// and delete local file
    			FileInfo local = new FileInfo(localDataDir + filename);
                local.Delete();
    
                ftp.Close();
            }
            catch (Exception ex)
            {
                log.Error("ResumePut() failed", ex);
                throw;
            }
		}
		
		/// <summary>Test resuming when putting a binary file</summary>
        [Test]
        public virtual void ResumeGet()
		{
			log.Debug("ResumeGet()");
            try
            {
				Connect();
    			
    			// move to test directory
    			ftp.ChangeWorkingDirectory(testdir);
    			ftp.TransferType = FTPTransferType.BINARY;
    			
    			// put to a random filename
    			string filename1 = GenerateRandomFilename();
    			
                // set up event handler to cancel the transfer
                cancelled = false;
                BytesTransferredHandler handler = new BytesTransferredHandler(CancelBytesTransferred);
    			ftp.BytesTransferred += handler;
    			
    			// initiate the get
                log.Debug("Getting '" + remoteBinaryFile + "' to '" + filename1 + "'");

                try
                {
   			        ftp.DownloadFile(localDataDir + filename1, remoteBinaryFile);
                    Assert.Fail("Should receive cancelled exception");
                }
                catch (FTPTransferCancelledException)
                {
                    log.Debug("Caught expected exception from cancelled transfer");
                }   
 
                // remove handler for next transfer
                ftp.BytesTransferred -= handler;

    			// should be cancelled - now resume
    			ftp.ResumeTransfer();
    			
    			// get again - should append
                log.Debug("Resuming '" + remoteBinaryFile + "' to '" + filename1 + "'");
    			ftp.DownloadFile(localDataDir + filename1, remoteBinaryFile);
    			
    			string filename2 = GenerateRandomFilename(); ;
    			
    			// do another get - complete this time    
    			ftp.DownloadFile(localDataDir + filename2, remoteBinaryFile);
    			
    			// check equality of local files
    			AssertIdentical(localDataDir + filename1, localDataDir + filename2);
    			
    			// and delete local files
    			FileInfo local = new FileInfo(localDataDir + filename1);
                local.Delete();
    			local = new FileInfo(localDataDir + filename2);
    			local.Delete();
    			
    			ftp.Close();
            }
            catch (Exception ex)
            {
                log.Error("ResumeGet() failed", ex);
                throw;
            }

		}

        [Test]
        public void TestResumeDownload()
        {
            log.Debug("TestResumeDownload()");
            try
            {
                Connect();

                // move to test directory
                ftp.ChangeWorkingDirectory(testdir);
                ftp.TransferType = FTPTransferType.BINARY;

                // put to a random filename
                string filename = GenerateRandomFilename();
                ftp.UploadFile(localDataDir + localBinaryFile, filename);
                long size = ftp.GetSize(filename);

                // get part of it back
                long resumePoint = (long)(new System.Random().NextDouble() * (double)size);
                ftp.ResumeNextDownload(resumePoint);

                FileStream str = new FileStream(localDataDir + localBinaryFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                MemoryStream ms = new MemoryStream();
                int count = 0;
                byte[] buf = new byte[4096];
                str.Seek(resumePoint, SeekOrigin.Begin);
                while ((count = str.Read(buf, 0, buf.Length)) > 0)
                {
                    ms.Write(buf, 0, count);
                }
                byte[] buf1 = ms.ToArray();
                byte[] buf2 = ftp.DownloadByteArray(filename);

                // delete remote file
                ftp.DeleteFile(filename);

                Assert.AreEqual(buf2.Length, size - resumePoint);
                AssertIdentical(buf1, buf2);

                ftp.Close();
            }
            catch (Exception ex)
            {
                log.Error("TestResumeDownload() failed", ex);
                throw;
            }
        }
	}
}
