var rau = {
    init: function() {
        rau.registerEvents();
    },
    registerEvents: function() {
        $('.abc').off('click').on('click',function(e) {
            
            e.preventDefault();
            var btn = $(this);
                var id = btn.data('id');

                $.ajax({
                    url: "/Admin/TrangThaiRau",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                    success: function(response) {
                        console.log(response);
                        if (response.status) {
                            btn.text("Kích hoạt");
                        } else {
                            btn.text("Khóa");
                        }
                            

                    }
                });
            });
    }   
}
rau.init();