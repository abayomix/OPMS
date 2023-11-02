﻿//using OPMS;
//using OPMS.Data;
using Repo_PMS.Abstract;
//using OPMS.Models;
using Repo_PMS.Models;
using Repo_PMS.Models.ModelMyAPI;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Utility_PMS;
using Utility_PMS.Model;


namespace Repo_PMS.Repository
{
    public class RepoPMS : IRepo
    {
        RepoMYAPI Apirepo = new RepoMYAPI();

        private ContextPMS _context = new ContextPMS();

        public user validateuser(Login login, string btn_click)
        {
            user userstatus = new user();

            if (btn_click == "Self LogIn")
            {
                UserDetail user = _context.UserDetails.Where(U => U.UserID == login.UserID).FirstOrDefault();

                if (user != null)
                {
                    try
                    {

                        if (user.Password == login.Password)
                        {
                            userstatus.userid = login.UserID;
                            userstatus.empId = user.EmpID;
                            userstatus.name = user.Name;
                            userstatus.role = getUserRole(user.UserID);
                            userstatus.IsVAliduser = "True";
                            userstatus.udeg = getUserDeg(user.UserID);
                            userstatus.udept = getUserDept(user.UserID);
                            userstatus.utype = "Self";
                            if (user.PasswordChangeDate != null)
                            {

                                userstatus.pwdchgstatus = "True";

                            }
                            else
                            {
                                userstatus.pwdchgstatus = "False";
                            }

                            userstatus.response = 200;
                        }
                        else
                        {
                            userstatus.userid = login.UserID;
                            userstatus.IsVAliduser = "False";
                            userstatus.response = 200;

                        }



                    }
                    catch (Exception E)
                    {
                        userstatus.response = 404;
                    }
                }
                else
                {
                    userstatus.response = 200;
                    return userstatus;
                }

            }

            return userstatus;


        }

        public string getUserDept(string userid)
        {
            string message;
            UserDetail user = _context.UserDetails.Where(U => U.UserID == userid).FirstOrDefault();
            try
            {
                if (user != null)
                {
                    Department d = _context.departments.Where(R => R.Id == Convert.ToInt32(user.DeptID)).FirstOrDefault();
                    message = d.DeptName;
                    return message;


                }
                else
                {
                    message = "No Record Found";
                    return message;
                }

            }
            catch (Exception E)
            {

                return E.Message;
            }





        }

        public string getUserRole(string userid)
        {
            string message;
            UserDetail user = _context.UserDetails.Where(U => U.UserID == userid).FirstOrDefault();
            try
            {
                if (user != null)
                {
                    RoleDetail r = _context.RoleDetails.Where(R => R.RoleId == Convert.ToInt32(user.RoleID)).FirstOrDefault();
                    message = r.RoleName;
                    return message;


                }
                else
                {
                    message = "No Record Found";
                    return message;
                }

            }
            catch (Exception E)
            {

                return E.Message;
            }



        }
        public string getUserDeg(string userid)
        {
            string message;
            UserDetail user = _context.UserDetails.Where(U => U.UserID == userid).FirstOrDefault();
            try
            {
                if (user != null)
                {
                    Degination r = _context.deginations.Where(R => R.Id == Convert.ToInt32(user.DeginationID)).FirstOrDefault();
                    message = r.DeginationName;
                    return message;


                }
                else
                {
                    message = "No Record Found";
                    return message;
                }

            }
            catch (Exception E)
            {

                return E.Message;
            }









        }

        public string getPasswordChangeStatus(string userid)
        {
            string message;
            UserDetail user = _context.UserDetails.Where(U => U.UserID == userid).FirstOrDefault();
            try
            {
                if (user != null)
                {
                    if (user.PasswordChangeDate != null)
                    {
                        message = "True";
                        return message;
                    }
                    else
                    {
                        message = "False";
                        return message;
                    }



                }
                else
                {
                    message = "No Record Found";
                    return message;
                }

            }
            catch (Exception E)
            {

                return E.Message;
            }



        }

        public string Changepassword(ChangePasswordVM changepassword)
        {
            string message;
            UserDetail user = _context.UserDetails.Where(U => U.UserID == changepassword.UserId).FirstOrDefault();

            if (changepassword.OldPassword == user.Password)
            {

                user.Password = changepassword.NewPasswod;
                user.PasswordChangeDate = DateTime.Now;
                _context.SaveChanges();
                message = "Sucess";
                return message;


            }
            else
            {
                message = "Unsucess";
                return message;
            }


        }

        public string AddUser(UserDetail User, string btn_click)
        {
            string message = null;
            if (btn_click == "Save To OPMS")
            {
                if (_context.UserDetails.Any(u => u.UserID == User.UserID))
                {
                    message = "Duplicate";
                }
                else
                {
                    try
                    {
                        if (User != null)
                        {
                            _context.UserDetails.Add(User);
                            _context.SaveChanges();

                            if (User.ID > 0)
                            {
                                Utilitys u = new Utilitys();
                                Email E = new Email();
                                E.Subject = "User Creation";
                                E.MEssage = string.Format("Dear {0} your Registration is successful. Your User ID is {1} and your password is {2}" + "Please visit the OPMS Portal to change your default password", User.Name, User.ID, User.Password);
                                E.To = User.Email;
                                u.SendEmail(E);
                                message = "Success";
                            }
                            else
                            {
                                message = "Unsuccessful";
                            }
                        }
                        else
                        {
                            message = "Oops you have filled all the required field";
                        }
                    }

                    catch (Exception E)
                    {
                        message = E.Message;
                    }
                }
            }
            else if (btn_click == "Save To API")
            {
                VmTupple vtm = Apirepo.Createuser(User).Result;
                if (vtm != null)
                {
                    Utilitys u = new Utilitys();
                    Email E = new Email();
                    E.Subject = "User Creation";
                    E.MEssage = string.Format("Dear {0} Your User Registration is Successful Your User ID is {1} and your password is {2}" + "Please go to Login and Change the default Password", vtm.VMC.Name, vtm.VMC.UserID, vtm.VMC.Password);
                    E.To = vtm.VMC.Email;
                    u.SendEmail(E);
                    message = vtm.status;
                }
                else
                {
                    message = vtm.status;
                }
            }

            return message;

        }

        public string AddRole(RoleDetail Role)
        {
            string message;
            try
            {
                if (Role != null)
                {
                    _context.RoleDetails.Add(Role);
                    _context.SaveChanges();
                    if (Role.RoleId > 0)
                    {
                        message = "Success";
                        return message;
                    }
                    else
                    {
                        message = "Unsuccessful";
                        return message;
                    }
                }

                else
                {
                    message = "Oops you filled all the required Fields";
                    return message;
                }
            }

            catch (Exception E)
            {
                message = E.Message;
                return message;
            }

        }

        public List<RoleDetail> GetAllRoles()
        {
            List<RoleDetail> lstRoles = _context.RoleDetails.ToList();
            return lstRoles;
        }
        public List<UserDetail> GetUsers()
        {
            List<UserDetail> lstusers = _context.UserDetails.ToList();
            return lstusers;
        }

        public List<UsersDLVM> GetUserForDL()
        {
            List<UsersDLVM> lstusers = _context.UserDetails.Select(u => new UsersDLVM { EmpID = u.EmpID, EmpName = u.Name }).ToList();
            return lstusers;
        }
        public List<Department> GetDepts()
        {
            List<Department> LstDepts = _context.departments.ToList();
            return LstDepts;
        }

        public List<Degination> GetDegination()
        {
            List<Degination> LstDeg = _context.deginations.ToList();
            return LstDeg;
        }

        public string AddDept(Department dept)
        {
            string message;
            try
            {
                if (_context.departments.Any(d => d.DeptName == dept.DeptName))
                {
                    message = "Duplicate";
                    return message;
                }
                else
                {
                    _context.departments.Add(dept);
                    _context.SaveChanges();
                    if (dept.Id > 0)
                    {
                        message = "Success";
                        return message;

                    }
                    else
                    {
                        message = "Failed";
                        return message;
                    }
                }
            }
            catch (Exception)
            {
                message = "Fail due to some Error";
                return message;
            }
        }

        public string AddDegination(Degination deg)
        {
            string message;
            try
            {
                if (_context.deginations.Any(d => d.DeginationName == deg.DeginationName))
                {
                    message = "Duplicate";
                    return message;
                }
                else
                {
                    _context.deginations.Add(deg);
                    _context.SaveChanges();
                    if (deg.Id > 0)
                    {
                        message = "Success";
                        return message;
                    }
                    else
                    {
                        message = "Failed";
                        return message;
                    }
                }
            }
            catch (Exception)
            {
                message = "Fail due to some Error";
                return message;
            }
        }

        public string AddReview(Review review)
        {
            string message;
            try
            {
                if (_context.reviews.Any(Review => Review.ReviewName == review.ReviewName))
                {
                    message = "You are trying to enter duplicate record";
                    return message;
                }
                else
                {
                    _context.reviews.Add(review);
                    _context.SaveChanges();
                    if (review.Id > 0)
                    {
                        message = "Success";
                        return message;
                    }
                    else
                    {
                        message = "Failed";
                        return message;
                    }
                }
            }
            catch (Exception)
            {
                message = "fail due to some error";
                return message;
            }
        }
        public string AddAssesment(assesment Assesment)
        {
            string message;
            try
            {
                if (_context.assesments.Any(A => A.AssessmentName == Assesment.AssessmentName))
                {
                    message = "You are tring to enter duplicate record";
                    return message;
                }
                else
                {
                    _context.assesments.Add(Assesment);
                    _context.SaveChanges();
                    if (Assesment.Id > 0)
                    {
                        message = "success";
                        return message;
                    }
                    else
                    {
                        message = "Failed";
                        return message;
                    }
                }
            }
            catch (Exception)
            {
                message = "Fail due to some error;" +
                    "return message";
                return message;
            }
        }
        public List<Review> GetAllReviews()
        {
            List<Review> reviews = _context.reviews.ToList();
            return reviews;

        }
        public Review GetReviewData(string Reviewid)
        {
            Review r = _context.reviews.Where(r => r.Id == int.Parse(Reviewid)).FirstOrDefault();
            return r;
        }
        public string updateReview(Review review)
        {
            Review r = _context.reviews.Where(r => r.Id == review.Id).FirstOrDefault();
            r.ReviewName = review.ReviewName;
            r.ReviewCategory = review.ReviewCategory;
            r.review_Description = review.review_Description;
            r.ReviewYear = review.ReviewYear;
            r.IsActive = review.IsActive;
            _context.SaveChanges();
            return "Success";
        }
        public string DeleteReview(Review review)
        {
            _context.Remove(review);
            _context.SaveChanges();
            return "Success";



        }
        public string ActDeactReview(string id)
        {
            Review r = _context.reviews.Where(r => r.Id == int.Parse(id)).FirstOrDefault();
            if (r.IsActive = false)
            {
                r.IsActive = true;
            }
            else
            {
                r.IsActive = false;
            }
            _context.Update(r);
            _context.SaveChanges();
            return "Success";
        }
        public List<assesment> GetAllAssesment()
        {
            List<assesment> lst = _context.assesments.ToList();
            return lst;
        }
        public assesment GetAssesmentData(string Assesmentid)
        {
            assesment A = _context.assesments.Where(A => A.Id == int.Parse(Assesmentid)).FirstOrDefault();
            return A;
        }
        public string updateAssesment(assesment assesment)
        {
            _context.Update(assesment);
            _context.SaveChanges();
            return "Success";
        }
        public string DeleteAssesment(assesment assesmet)
        {
            _context.Remove(assesmet);
            _context.SaveChanges();
            return "Sucess";

        }
        public string ActDeactAssement(string id)
        {
            assesment r = _context.assesments.Where(r => r.Id == int.Parse(id)).FirstOrDefault();
            if (r.IsActive == false)
            {
                r.IsActive = true;
            }
            else
            {
                r.IsActive = false;
            }
            _context.Update(r);
            _context.SaveChanges();
            return "Success";
        }

        public List<VMScoreCard> ShowFinalScore(string year)
        {
            List<VMScoreCard> Scores = new List<VMScoreCard>();
            var Assesmentrecords = _context.AssesmentResponses.ToList().GroupBy(G => G.Empid);
            var FeedbackRecords = _context.feedbacks.ToList().GroupBy(f => f.empid);

            foreach (var records in Assesmentrecords)
            {
                VMScoreCard vms = new VMScoreCard();
                vms.EmpID = records.Key;
                vms.UserID = _context.UserDetails.FirstOrDefault(f => f.EmpID == records.Key).UserID;
                vms.Name = _context.UserDetails.FirstOrDefault(f => f.EmpID == records.Key).Name;
                if (FeedbackRecords.Any(f => f.Key == records.Key
                ))

                {
                    decimal countfeedback = FeedbackRecords.FirstOrDefault(f => f.Key == records.Key).Count();
                    decimal vlalueonepercent = Math.Round(countfeedback / 100 * 10, 2);
                    vms.F_Score = Math.Round(FeedbackRecords.FirstOrDefault(f => f.Key == records.Key).Sum(f => f.score) / vlalueonepercent, 2);
                }
                decimal countAssmentQuestion = records.Count();
                decimal AssValueonepercen = Math.Round(countAssmentQuestion / 100, 2);

                if (AssValueonepercen > 0)
                {
                    int countcorrect = records.Count(r => r.iscorrect == true);
                    vms.A_Score = Math.Round(countcorrect / AssValueonepercen, 2);

                }
                if (vms.F_Score > 0 && vms.A_Score > 0)
                {
                    AssesmentReviewRatio asr = _context.AssesmentReviewRatios.FirstOrDefault(ar => ar.Year == year);
                    vms.AggrScore = Math.Round(vms.AggrScore / 100 * asr.ReviewRatio + vms.A_Score / 100 * asr.AssesmentRatio, 2);
                }
                Scores.Add(vms);


            }
            return Scores;







        }
        public List<VMScoreCard> ShowFinalScoreV2(int dept, int deg, string year)
        {
            int AssmentTotalQuestion = 0;
            var assementids = new List<int>();
            List<VMScoreCard> Scores = null;
            var empids = new List<string>();
            IEnumerable<IGrouping<string, AssesmentResponse>>? AssementRecords = null;
            IEnumerable<IGrouping<string, Feedback>>? FeedbackRecords = null;
            if(_context.assesments.Any(A => A.AssessmentYear == year))
            {
                assementids = _context.assesments.Where(A => A.AssessmentYear == year). Select(A => A.Id).ToList(); 
            }
            if(dept == 0)
            {
                if(deg == 0)
                {
                    if(_context.UserDetails.Any())
                    {
                        Scores = _context.UserDetails.Select(f => new VMScoreCard { UserID = f.UserID, EmpID = f.EmpID, Name = f.Name }).ToList();
                    }
                }
                else
                {
                    if(_context.UserDetails.Any(U => U.DeginationID == deg))
                    {
                        Scores = _context.UserDetails.Where(U => U.DeginationID == deg).Select(f => new VMScoreCard { UserID = f.UserID, EmpID = f.EmpID, Name = f.Name}).ToList() ;
                    }
                }
            }
            else
            {
                if(deg == 0)
                {
                    if (_context.UserDetails.Any(U =>U.DeptID == dept))
                    {
                        Scores = _context.UserDetails.Where(U => U.DeptID == dept).Select(f => new VMScoreCard { UserID = f.UserID, EmpID = f.EmpID, Name = f.Name }).ToList();
                    }

                }
                else
                {
                    if(_context.UserDetails.Any(U => U.DeptID == dept && U.DeginationID == deg))
                    {
                        Scores = _context.UserDetails.Where(U => U.DeptID == dept && U.DeginationID == deg).Select(f => new VMScoreCard { UserID = f.UserID, EmpID = f.EmpID, Name = f.Name }).ToList() ;   
                    }
                }
            }
            if (Scores !=null)
            {
                empids = Scores.Select(s => s.EmpID).ToList();
            }

            if(_context.AssesmentResponses.Any(A => empids.Contains(A.Empid)))
            {
                AssementRecords = _context.AssesmentResponses.Where(A => empids.Contains(A.Empid)).ToList().GroupBy(G => G.Empid);
            }
            if (_context.feedbacks.Any(A => empids.Contains(A.empid)))
            {
                FeedbackRecords = _context.feedbacks.Where(A => empids.Contains(A.empid)).ToList().GroupBy(f => f.empid);
            }
            if (AssementRecords != null)
            {
                foreach (var records in AssementRecords)
                {
                    VMScoreCard vms = Scores.FirstOrDefault(S => S.EmpID == records.Key);
                    int DepID = _context.UserDetails.FirstOrDefault(F => F.EmpID == vms.EmpID).DeptID;
                    int DegID = _context.UserDetails.FirstOrDefault(F => F.EmpID == vms.EmpID).DeginationID;
                    foreach (var i in assementids)
                    {
                       if(_context.QuestionSets.Any(Q => Q.AssesmentID == i && Q.Dept == DepID && Q.Deg == DegID))
                        {
                            string? s = _context.QuestionSets.FirstOrDefault(Q => Q.AssesmentID == i && Q.Dept == DepID && Q.Deg == DegID).QuestionID; if(s != null)
                            {
                                List<int> questionids =s.Split(',').Select (int.Parse).ToList();
                                AssmentTotalQuestion = AssmentTotalQuestion + questionids.Count;
                            }
                        }
                    }
                    decimal countAssmentQuestion = AssmentTotalQuestion;
                    decimal AssValueonepercen = Math.Round(countAssmentQuestion / 100, 2);
                    if (AssValueonepercen > 0)
                    {
                        int countcorrect = records.Count(r => r.iscorrect == true);
                        vms.A_Score = Math.Round(countcorrect / AssValueonepercen, 2);

                    }

                }
            }
            if (FeedbackRecords !=null) 
            {
                foreach (var records in FeedbackRecords)
                {
                    VMScoreCard vms = Scores.FirstOrDefault(S => S.EmpID == records.Key);
                    if (FeedbackRecords.Any(f => f.Key == records.Key)) 
                    {
                        decimal countfeedback = FeedbackRecords.FirstOrDefault(f => f.Key == records.Key).Count();
                        decimal vlaueonepercent = Math.Round(countfeedback / 100 - 10, 2);
                        vms.F_Score = Math.Round(FeedbackRecords.FirstOrDefault(f => f.Key == records.Key).Sum(f => f.score) / vlaueonepercent, 2);
                    }    
                }
            }

            AssesmentReviewRatio? asr = _context.AssesmentReviewRatios.Where(ar => ar.Year == year).FirstOrDefault();
            if (Scores != null)
            {
                foreach (var records in Scores)
                {
                    if(records.F_Score > 0 && records.A_Score > 0)
                    {
                        records.AggrScore = Math.Round(records.F_Score / 100 * records.A_Score / 100 * asr.AssesmentRatio, 2);
                    }
                }
            }

            return Scores;
        }


    }
}
