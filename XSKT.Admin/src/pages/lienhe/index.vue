<script>
import Layout from "@/pages/congdan/layout";
import {latLng} from "leaflet";
import {LMap, LTileLayer, LMarker, LTooltip, LIcon} from "vue2-leaflet";
import {mapState} from "vuex";

/**
 * Crypto ICO-landing page
 */
export default {
  components: {
    Layout,
    LMap,
    LTileLayer,
    LMarker,
    LIcon,
    LTooltip,
  },
  data() {
    return {
      perPage: 10,
      currentPage: 1,
      sortBy: "age",
      sortDesc: false,
      filterOn: [],
      zoom: 10,
      marker1LatLng: [10.466849124651592, 105.64187049356956],
      marker2LatLng: [10.7762751, 106.6838469],
      marker1PopupText: 'Điểm đánh dấu 1',
      marker2PopupText: 'Điểm đánh dấu 2',
      center: latLng(10.648007, 106.2000751),
      url: "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
      attribution:
          '&copy; <a href="http://osm.org/copyright">2022</a> Tỉnh Đồng tháp',
      withTooltip: latLng(10.466849124651592, 105.64187049356956),
      showParagraph: false,
      mapOptions: {
        zoomSnap: 0.5,
      },
      showMap: true,
      model: [],
      listMenu : [],
      menu : null,
    };
  },
  created() {
    this.getGioiThieu();
    this.getMenu();
  //  console.log("LOG CREATED LIEN HE ")
    window.addEventListener("scroll", this.windowScroll);
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  methods: {
    async getMenu(){
      let menuStorage = localStorage.getItem("flatten-menu");
   //   console.log("LOG GET MENU STOREAGE :  ",menuStorage)
      if (menuStorage) {
        this.listMenu= JSON.parse(menuStorage);
        this.menu =  JSON.parse(menuStorage).find(item => item.link ===window.location.pathname);
       // console.log("LOG ROUTER :  ", this.menu)

        this.getGioiThieu();
      }
    },
    async getGioiThieu(){
      // const params = {
      //   start: this.currentPage,
      //   limit: this.perPage,
      //   sortDesc: true,
      //   menuId: this.menu.id,
      // };
      // await this.$store.dispatch("contentStore/getByMenuId",params ).then((res) => {
      // //  console.log("MENU: ", res.data)
      //   this.model = res.data;
      //   if (res.data.data && res.data.length > 0) {
      // // Lấy phần tử cuối cùng của mảng
      //     const lastIndex = res.data.length - 1;
      //     this.model = res.data[lastIndex];
      //   }
      // })
    },
    handleShowRegisterModal(){
      this.$store.dispatch("snackBarStore/showRegisterModal", !this.$store.state.snackBarStore.registerModal)
  //    console.log("abc")
    },
    timerCount: function (start, end) {
      // Get todays date and time
      var now = new Date().getTime();

      // Find the distance between now an the count down date
      var distance = start - now;
      var passTime = end - now;

      if (distance < 0 && passTime < 0) {
        clearInterval(this.interval);
        return;
      } else if (distance < 0 && passTime > 0) {
        this.calcTime(passTime);
      } else if (distance > 0 && passTime > 0) {
        this.calcTime(distance);
      }
    },
    calcTime: function (dist) {
      // Time calculations for days, hours, minutes and seconds
      this.days = Math.floor(dist / (1000 * 60 * 60 * 24));
      this.hours = Math.floor(
          (dist % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
      );
      this.minutes = Math.floor((dist % (1000 * 60 * 60)) / (1000 * 60));
      this.seconds = Math.floor((dist % (1000 * 60)) / 1000);
    },
    /**
     * Window scroll method
     */
    windowScroll() {
      const navbar = document.getElementById("navbar");
      if (
          document.body.scrollTop >= 50 ||
          document.documentElement.scrollTop >= 50
      ) {
        navbar.classList.add("nav-sticky");
      } else {
        navbar.classList.remove("nav-sticky");
      }
    },
    /**
     * Toggle menu
     */
    toggleMenu() {
      document.getElementById("topnav-menu-content").classList.toggle("show");
    },
    nextSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getNextPage());
    },
    prevSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getPreviousPage());
    },
    openPopup(event) {
      event.target.openPopup();
    },
  }
};
</script>

<template>
  <Layout>


    <section class="section p-1 bg-white" id="about">

      <div class="container" style="padding: 0px 50px">
        <div class="row">
          <div class="col-12 mb-2">
              <div class="cs-title-box">
                <div class="cs-title py-2">
                  <a href="">
                    <i class="mdi mdi-star ic-item"></i>
                    <span style="color: #fff; font-size: 16px;">LIÊN HỆ</span>
                  </a>
                </div>
              </div>
            </div>
          <!-- <div class="col-12 mb-2">
            <h3 class="fw-bold" style="color: rgb(214, 6, 4); padding: 10px; text-transform: uppercase;">{{ this.model.name }}</h3>
          </div> -->
          <div class="col-md-12">
            <span v-html="model.noiDung"></span>
          </div>
          <div class="col-12 shadow-sm mb-3">
            <l-map
                :zoom="zoom"
                :center="center"
                :options="mapOptions"
                style="height: 600px; width: 100%"
            >
              <l-tile-layer :url="url" :attribution="attribution"/>
              <l-marker :lat-lng="withTooltip" @click="openPopup">
                <l-tooltip :options="{ permanent: true, interactive: true }">
                  <div class="text-dark p-2">
                    <h4 class="text-dark">Công ty Xổ số kiến thiết Đồng Tháp</h4>
                    <p>Số 01, đường Duy Tân, phường Mỹ Phú, TP Cao Lãnh
                      <br> tỉnh Đồng Tháp</p>
                    <a href="https://www.google.com/maps/place/C%C3%B4ng+ty+TNHH+m%E1%BB%99t+th%C3%A0nh+vi%C3%AAn+X%E1%BB%95+s%E1%BB%91+ki%E1%BA%BFn+thi%E1%BA%BFt+%C4%90%E1%BB%93ng+Th%C3%A1p/@10.4671438,105.6418124,18.77z/data=!4m14!1m7!3m6!1s0x310a644b5431fa73:0xd218f849d97fb3ec!2zMSBEdXkgVMOibiwgUGjGsOG7nW5nIE3hu7kgUGjDuiwgVFAuIENhbyBMw6NuaCwgxJDhu5NuZyBUaMOhcCwgVmnhu4d0IE5hbQ!3b1!8m2!3d10.4694188!4d105.6414281!3m5!1s0x310a7f40c631e92b:0x6c7347a318978f9b!8m2!3d10.4667886!4d105.6423541!16s%2Fg%2F11ftw779zl?hl=vi-VN&entry=ttu">
                      Xem bản đồ lớn hơn
                    </a>
                  </div>
                </l-tooltip>
                <l-icon>
                  <img src="@/assets/images/leaflet/marker-icon.png"/>
                </l-icon>
              </l-marker>
              <l-marker :lat-lng="marker2LatLng" @click="openPopup">
                <l-tooltip :options="{ permanent: true, interactive: true }">
                  <div class="text-dark p-2">
                    <h4 class="text-dark">Văn phòng đại diện tại TP.HCM</h4>
                    <p>Số 03, Phạm Đình Toái, phường Võ Thị Sáu, quận 3
                      <br> TP. Hồ Chí Minh</p>
                    <a href="https://www.google.com/maps/place/3+%C4%90+Ph%E1%BA%A1m+%C4%90%C3%ACnh+To%C3%A1i,+Ph%C6%B0%E1%BB%9Dng+6,+Qu%E1%BA%ADn+3,+Th%C3%A0nh+ph%E1%BB%91+H%E1%BB%93+Ch%C3%AD+Minh,+Vi%E1%BB%87t+Nam/@10.7762816,106.6838393,17z/data=!3m1!4b1!4m6!3m5!1s0x31752f24e1d5eead:0xbb4702019e2af816!8m2!3d10.7762816!4d106.6864142!16s%2Fg%2F11j8gzlvj7?hl=vi-VN&entry=ttu">
                      Xem bản đồ lớn hơn
                    </a>
                  </div>
                </l-tooltip>
                <l-icon>
                  <img src="@/assets/images/leaflet/marker-icon.png"/>
                </l-icon>
              </l-marker>
            </l-map>
          </div>
        </div>
      </div>
    </section>
  </Layout>
</template>
<style>
.color-primary {
  /*color: #28883B;*/
  color: #cc3333
}

.bg-primary {
  /*background-color: #28883B !important;*/
  background-color:#cc3333 !important;
}

.lienhe-box {
  margin-top: -100px;
}

.lh-phone,
.lh-gmail {
  border-radius: 10px;
}

.lh-phone > img,
.lh-gmail > img {
  width: 150px;
}

.lh-icon {
  font-size: 100px !important;
}

.cate-title{
  background-color: #05912a;
  color: #fff;
  padding: 10px;
  font-size: 20px;
}

.cate-list ul li{
  list-style: none;
  border-bottom: 1px dashed #d0cfcf;
  padding: 10px 0 10px 0;
}

.cate-list ul li a{
  margin-left: 10px;
  font-size: 14px;
  color: #78797C;
}

/* .cs-title-box .cs-title .ic-item {
  background-color: #fff;
  color: #d60604;
  padding: 5px 7px;
  border-radius: 50px;
  margin-right: 10px;
}

.cs-title-box .cs-title {
  background-color: #d60604;
  color: #fff;
  width: fit-content;
  padding: 5px;
  padding-right: 20px;
  border-radius: 50px;
  position: relative;
  z-index: 99;
  margin: 10px 0px;

}

.cs-title-box:before {
  display: block;
  height: 1px;
  width: 100%;
  background: linear-gradient(90deg, #d60604, rgba(199, 26, 22, 0));
  position: absolute;
  top: 50%;
  z-index: 1;
} */

</style>
