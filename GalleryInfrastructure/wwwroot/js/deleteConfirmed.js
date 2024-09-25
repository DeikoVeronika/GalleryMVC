document.getElementById('deleteForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const form = this;
    const formData = new FormData(form);

    fetch(form.action, {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Успіх!',
                    text: data.message, 
                    confirmButtonText: 'ОК'
                }).then(() => {
                    window.location.href = form.getAttribute('data-return-url'); 
                });
            } else if (data.confirmRequired) {
                //підтвердження (для видалення автора з фото)
                Swal.fire({
                    icon: 'warning',
                    title: 'Підтвердіть видалення',
                    text: data.message, 
                    showCancelButton: true,
                    confirmButtonText: 'Так, видалити',
                    cancelButtonText: 'Скасувати'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Якщо користувач підтвердив видалення, повторно надсилаємо форму з параметром підтвердження
                        formData.append('confirmDeletePhotos', 'true');

                        fetch(form.action, {
                            method: 'POST',
                            body: formData,
                            headers: {
                                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                            }
                        })
                            .then(response => response.json())
                            .then(data => {
                                if (data.success) {
                                    Swal.fire({
                                        icon: 'success',
                                        title: 'Успіх!',
                                        text: data.message, 
                                        confirmButtonText: 'ОК'
                                    }).then(() => {
                                        window.location.href = form.getAttribute('data-return-url'); 
                                    });
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Помилка!',
                                        text: data.message, 
                                        confirmButtonText: 'ОК'
                                    });
                                }
                            })
                            .catch(error => {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Сталася помилка!',
                                    text: 'Неможливо видалити запис.',
                                    confirmButtonText: 'ОК'
                                });
                            });
                    }
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Помилка!',
                    text: data.message, 
                    confirmButtonText: 'ОК'
                });
            }
        })
        .catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'Сталася помилка!',
                text: 'Неможливо видалити запис.',
                confirmButtonText: 'ОК'
            });
        });
});
