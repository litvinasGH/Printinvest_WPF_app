using Printinvest_WPF_app.Models;
using Printinvest_WPF_app.Repositories;
using Printinvest_WPF_app.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Printinvest_WPF_app.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private readonly CartRepository _cartRepository;
        private readonly OrderRepository _orderRepository;
        private Cart _cart;
        private ObservableCollection<CartItem> _cartItems;
        private string _recipientName;
        private string _deliveryAddress;
        private string _phoneNumber;
        private ObservableCollection<string> _paymentMethods;
        private string _selectedPaymentMethod;

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        public string RecipientName
        {
            get => _recipientName;
            set => SetProperty(ref _recipientName, value);
        }

        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set => SetProperty(ref _deliveryAddress, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public ObservableCollection<string> PaymentMethods
        {
            get => _paymentMethods;
            set => SetProperty(ref _paymentMethods, value);
        }

        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set => SetProperty(ref _selectedPaymentMethod, value);
        }

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand PlaceOrderCommand { get; }

        public CartViewModel()
        {

            _cartRepository = RepositoryManager.Carts;
            _orderRepository = RepositoryManager.Orders;

            CartItems = new ObservableCollection<CartItem>();
            PaymentMethods = new ObservableCollection<string> { "Картой", "Наличными" };
            SelectedPaymentMethod = PaymentMethods.FirstOrDefault();

            IncreaseQuantityCommand = new RelayCommandSec(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommandSec(DecreaseQuantity);
            RemoveItemCommand = new RelayCommandSec(RemoveItem);
            PlaceOrderCommand = new RelayCommand(PlaceOrder);

            LoadCart();
        }

        private void LoadCart()
        {
            try
            {
                if (!SessionManager.IsAuthenticated)
                {
                    MessageBox.Show("Пожалуйста, войдите в систему для просмотра корзины.", "Авторизация требуется", MessageBoxButton.OK, MessageBoxImage.Warning);
                    
                    return;
                }

                _cart = _cartRepository.GetByUserId(SessionManager.CurrentUser.Id);
                if (_cart == null)
                {
                    _cart = new Cart
                    {
                        UserId = SessionManager.CurrentUser.Id,
                        User = SessionManager.CurrentUser,
                        Items = new List<CartItem>()
                    };
                    _cartRepository.Add(_cart);
                }

                CartItems.Clear();
                foreach (var item in _cart.Items)
                {
                    CartItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки корзины: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IncreaseQuantity(object parameter)
        {
            try
            {
                if (parameter is CartItem item)
                {
                    Console.WriteLine($"Увеличение количества для CartItem Id: {item.Id}");
                    item.Quantity++;
                    _cartRepository.Update(_cart);
                    OnPropertyChanged(nameof(CartItems)); // Уведомляем UI об изменении
                    Console.WriteLine($"Новое количество: {item.Quantity}");
                }
                else
                {
                    Console.WriteLine("Параметр IncreaseQuantity не является CartItem");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка изменения количества: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DecreaseQuantity(object parameter)
        {
            try
            {
                if (parameter is CartItem item && item.Quantity > 1)
                {
                    Console.WriteLine($"Уменьшение количества для CartItem Id: {item.Id}");
                    item.Quantity--;
                    _cartRepository.Update(_cart);
                    OnPropertyChanged(nameof(CartItems)); // Уведомляем UI об изменении
                    Console.WriteLine($"Новое количество: {item.Quantity}");
                }
                else
                {
                    Console.WriteLine("Параметр DecreaseQuantity не является CartItem или Quantity <= 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка изменения количества: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveItem(object parameter)
        {
            try
            {
                if (parameter is CartItem item)
                {
                    Console.WriteLine($"Удаление CartItem Id: {item.Id}");
                    _cartRepository.RemoveItemFromCart(_cart.Id, item.Id);
                    CartItems.Remove(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления элемента: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlaceOrder()
        {
            try
            {
                if (SessionManager.CurrentUser == null)
                {
                    MessageBox.Show("Пожалуйста, войдите в систему для оформления заказа.", "Авторизация требуется", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!CartItems.Any())
                {
                    MessageBox.Show("Корзина пуста.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(RecipientName) ||
                    string.IsNullOrWhiteSpace(DeliveryAddress) ||
                    string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    MessageBox.Show("Заполните все поля доставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedPaymentMethod))
                {
                    MessageBox.Show("Выберите способ оплаты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var order = new Order
                {
                    UserId = SessionManager.CurrentUser.Id,
                    User = SessionManager.CurrentUser,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.Now,
                    Items = new List<OrderItem>()
                };

                foreach (var cartItem in CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        ServiceId = cartItem.ServiceId,
                        Product = cartItem.Product,
                        Service = cartItem.Service,
                        Quantity = cartItem.Quantity
                    };
                    order.Items.Add(orderItem);
                }

                _orderRepository.Add(order);

                // Очистка корзины
                _cart.Items.Clear();
                _cartRepository.Update(_cart);
                CartItems.Clear();

                MessageBox.Show("Заказ успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оформления заказа: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}