﻿document.addEventListener('DOMContentLoaded', function () {
    const today = new Date().toISOString().split("T")[0];
    document.getElementById('dateInput').setAttribute('max', today);
});