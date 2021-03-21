using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IQAService
    {
        QA AddQA(QA qA);
        IEnumerable<QA> GetQAByProductID(int ID);
        QA GetQAByID(int ID);
        void UpdateQA(QA qA);
        IEnumerable<QA> GetQAList();
    }
    public class QAService : IQAService
    {
        private readonly UnitOfWork context;
        public QAService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public QA AddQA(QA qA)
        {
            this.context.QARepository.Insert(qA);
            return qA;
        }

        public QA GetQAByID(int ID)
        {
            QA qA = this.context.QARepository.GetDataByID(ID);
            return qA;
        }

        public IEnumerable<QA> GetQAByProductID(int ID)
        {
            IEnumerable<QA> listQA = this.context.QARepository.GetAllData(x => x.ProductID == ID);
            return listQA;
        }

        public IEnumerable<QA> GetQAList()
        {
            IEnumerable<QA> listQA = this.context.QARepository.GetAllData();
            return listQA;
        }

        public void UpdateQA(QA qA)
        {
            qA.DateAnswer = DateTime.Now;
            qA.Status = true;
            this.context.QARepository.Update(qA);
        }
    }
}