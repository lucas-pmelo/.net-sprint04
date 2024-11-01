using Sprint03.infra.exception;
using Sprint03.domain.model;
using Sprint03.domain.repository;
using Sprint03.infra.service.dto;
using Sprint03.domain.useCase.dto;

namespace Sprint03.domain.useCase
{
    public class CustomerUseCase : ICustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICepValidationService _cepValidationService;

        public CustomerUseCase(ICustomerRepository customerRepository, ICepValidationService cepValidationService)
        {
            _customerRepository = customerRepository;
            _cepValidationService = cepValidationService;
        }

        public Customer FindById(string id)
        {
            var customer = _customerRepository.FindById(id);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found.");
            }

            return customer;
        }

        public void Create(Customer customer)
        {
            var persistedCustomer = _customerRepository.FindById(customer.Id);

            if (persistedCustomer != null)
            {
                throw new AlreadyExistsException($"Customer with ID {customer.Id} already exists.");
            }

            _customerRepository.Create(customer);
        }

        public Customer Update(string id, Customer customer)
        {
            var persistedCustomer = _customerRepository.FindById(id);

            if (persistedCustomer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found.");
            }

            _customerRepository.Update(id, customer);

            return customer;
        }

        public void Delete(string id)
        {
            var persistedCustomer = _customerRepository.FindById(id);

            if (persistedCustomer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found.");
            }

            _customerRepository.Delete(id);
        }
        
        public async Task<bool> ValidateCepAsync(string cep)
        {
            return await _cepValidationService.IsValidCepAsync(cep);
        }

    }
}