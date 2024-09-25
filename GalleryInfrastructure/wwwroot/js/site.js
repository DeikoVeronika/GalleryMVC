// Очищення повідомлень про помилки при введенні даних
const inputs = document.querySelectorAll('.form-control');
inputs.forEach(input => {
    input.addEventListener('input', function () {
        const errorSpan = this.nextElementSibling;
        if (errorSpan) {
            errorSpan.textContent = '';
        }
    });
});

