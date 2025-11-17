
function downloadPDF() {
    const { jsPDF } = window.jspdf;
    const pdf = new jsPDF('p', 'mm', 'a4');

    const pages = document.querySelectorAll(".page");

    // تحديد اسم الملف
    let pdfName = "servant-cards.pdf";

    // لو فيه select لكنيسة، خد الاسم
    const churchDropdown = document.getElementById("churchSelect");
    if (churchDropdown) {
        const selectedText = churchDropdown.options[churchDropdown.selectedIndex].text;
        pdfName = `كروت-${selectedText}.pdf`;
    } else {
        // لو مفيش dropdown، خد اسم الخادم الأول
        const servantName = document.querySelector(".servant-name");
        if (servantName) {
            const name = servantName.innerText.trim().replace(/\s+/g, "-");
            pdfName = `كارت-${name}.pdf`;
        }
    }

    // إظهار حالة التحميل
    const printButton = document.getElementById("printButton");
    const originalText = printButton.innerText;
    printButton.innerText = "جاري التحميل...";
    printButton.disabled = true;

    const generatePage = async (page, index) => {
        return html2canvas(page, {
            scale: 2,
            useCORS: true
        }).then(canvas => {
            const imgData = canvas.toDataURL("image/jpeg", 1.0);
            const pageWidth = pdf.internal.pageSize.getWidth();
            const pageHeight = pdf.internal.pageSize.getHeight();

            const imgProps = pdf.getImageProperties(imgData);
            const imgRatio = imgProps.width / imgProps.height;
            const pdfRatio = pageWidth / pageHeight;

            let imgWidth, imgHeight;

            if (imgRatio > pdfRatio) {
                imgWidth = pageWidth;
                imgHeight = pageWidth / imgRatio;
            } else {
                imgHeight = pageHeight;
                imgWidth = pageHeight * imgRatio;
            }

            if (index !== 0) pdf.addPage();
            pdf.addImage(imgData, 'JPEG', 0, 0, imgWidth, imgHeight);
        });
    };

    (async () => {
        for (let i = 0; i < pages.length; i++) {
            await generatePage(pages[i], i);
        }
        pdf.save(pdfName);
        printButton.innerText = originalText;
        printButton.disabled = false;
    })();
}

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('img').forEach(img => {
        img.onerror = function () {
            if (this.alt.includes('صورة الخادم')) {
                this.parentElement.style.display = 'none';
            } else if (this.alt.includes('باركود')) {
                this.style.display = 'none';
            }
        };
    });
});

