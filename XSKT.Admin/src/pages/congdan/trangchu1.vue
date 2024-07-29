<script>
import {Carousel, Slide} from "vue-carousel";
import Layout from "@/pages/congdan/layout2";
// import the component
// import the styles
import "@riophae/vue-treeselect/dist/vue-treeselect.css";
import {mapState} from "vuex";
import 'vue-slick-carousel/dist/vue-slick-carousel.css'
import 'vue-slick-carousel/dist/vue-slick-carousel-theme.css'
import { ketQuaXSModel } from "@/models/ketQuaXSModel";
import moment from 'moment-timezone';
export default {
  components: {
    Layout,
  },
  data() {
    return {
      apiView: `${process.env.VUE_APP_API_URL}filesminio/view/`,
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
      ImagesShow: [],
      totalRows: 1,
      numberOfElement: 1,
      perPage: 5,
      currentPage: 1,
      sortBy: "age",
      sortDesc: false,
      filterOn: [],
      showModal: true,
      checked: false,
      submitted: false,
      currentPlace: null,
      showCurrentPlace: true,
      selection: "all",
      visible: false,
      listFileSilder : [],
      suKien:[],
      lienket:[],
      listtin:[],
      listthongbao:[],
      thuvienanh:[],
      ketquaxoso: null,
      imagehome: [],
      lienketwebsite:[],
      diemban:[],
      diembanloto:[],
      relink:[],
      video:[],
      bienban:[],
      tinnoibat: [],
      tindulich:[],
      tinansinhxahoi: [],
      tinhoatdongcongty: [],
      tinhoatdongdoanthe: [],
      modelKQ: ketQuaXSModel.baseJson(),
      selectedItem: null,
      locale: 'vi',
      loaiAnh: [],
    };
  },

  computed: {
    ...mapState('authStore', ['token']),

    //Validations
  },
  validations() {
    return {
      model: this.rules
    }
  },
  created() {

    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: ['admin', 'owner', 'moderator', 'create-post', 'edit-post', 'delete-post']
    }
    // this.getThongTinSlider();
    this.getSuKien();
    this.getLink();
    this.getTinTrangChu();
    this.getThongBao();
    this.getThuVienAnh();
    this.getKQXoSo();
    this.getImageHome();
    this.getLinkWebsite();
    this.getDiemBan();
    this.getDiemBanLoTo();
    this.getVideo();
    this.getBienBan();
    this.getTinNoiBat();
    this.getTinDuLich();
    this.getTinAnSinhXaHoi();
    this.getTinHoatDongCongTy();
    this.getTinHoatDongDoanThe();
    this.getLoaiAnh();

    // setInterval(() => {
    //   this.getKQXoSo();
    // }, 60000);
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  mounted() {
    this.reloadIfNeeded();

    setInterval(() => {
      this.reloadIfNeeded();
    }, 30000);
  },
  watch: {
    token: {
      deep: true,
      handler(val) {
        this.tokenCurrent = val;
      }
    },
  },

  methods: {
    clearDate() {
      this.modelKQ.date = null;
    },
    // Hàm để lấy giờ hiện tại theo múi giờ UTC
    // getCurrentUTCHours() {
    //   const now = new Date();
    //   return now.getUTCHours();
    // },

    // Hàm để kiểm tra xem có phải là thời điểm cần load lại không
    shouldReloadData() {
      const now = moment().tz('Asia/Ho_Chi_Minh'); // Đặt múi giờ cho Việt Nam

      // Kiểm tra nếu là thứ 2, và giờ hiện tại nằm trong khoảng từ 16:30 đến 17:30
      const isMonday = now.isoWeekday() === 1;
      const isWithinTimeRange = now.hours() === 16 && now.minutes() >= 10 && now.hours() === 16 && now.minutes() <= 50;

      return isMonday && isWithinTimeRange;
    },

    // Hàm để thực hiện load lại dữ liệu
    reloadIfNeeded() {
      if (this.shouldReloadData()) {
        this.getKQXoSo();
      }
    },
    async getTinNoiBat(){
      const params = {
        start: 0,
        limit: 6,
        sortDesc: true,
        menuId: "655dc9425d377463faffe79c",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        this.tinnoibat = res.data.data;
      })
    },
    async getTinDuLich(){
      const params = {
        start: 0,
        limit: 4,
        sortDesc: true,
        menuId: "655c2580f006435d39df0b15",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        this.tindulich = res.data.data;
      })
    },
    async getTinAnSinhXaHoi(){
      const params = {
        start: 0,
        limit: 1,
        sortDesc: true,
        menuId: "65484358a202972a89722ac4",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        this.tinansinhxahoi = res.data.data;
      })
    },
    async getTinHoatDongCongTy(){
      const params = {
        start: 0,
        limit: 1,
        sortDesc: true,
        menuId: "65f7b1f2520e9c543595b971",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        this.tinhoatdongcongty = res.data.data;
      })
    },
    async getTinHoatDongDoanThe(){
      const params = {
        start: 0,
        limit: 1,
        sortDesc: true,
        menuId: "654842dfa202972a89722ac2",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        this.tinhoatdongdoanthe = res.data.data;
      })
    },
    async getBienBan() {
      const params = {
        start: 0,
        limit: 4,
      };
      await this.$store.dispatch("bienBanXoSoStore/getFour", params).then((res) => {
        this.bienban = res.data.data;
      })
    },
    async getImageHome(){
      await this.$store.dispatch("imageHomeStore/getAll").then((res) => {
        this.imagehome = res.data;
      })
    },
    async getThuVienAnh() {
      await this.$store.dispatch("thuvienanhStore/getAll").then((res) => {
        this.thuvienanh = res.data;
      })
    },
    async getLoaiAnh(){
      await this.$store.dispatch("loaiAnhStore/getAll").then((res) => {
        console.log("LOAI ANH: ", res.data);
        this.loaiAnh = res.data;
      })
    },
    async getThongBao(){
      const params = {
        start: this.currentPage,
        limit: this.perPage,
        sortDesc: true,
        menuId: "655c279fce5d0ab0f44b3edc",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        this.listthongbao = res.data.data;
      })
    },
    async getTinTrangChu(){
      await this.$store.dispatch("contentStore/getTinTrangChu").then((res) => {
        this.listtin = res.data;
      })
    },
    async getLink(){
      const params = {
        start: 0,
        limit: 4,
        sortBy: 'thuTu',
      };
      await this.$store.dispatch("linkStore/getPagingParams", params).then((res) => {
        this.lienket = res.data.data;
      })
    },
    async getSuKien(){
      await this.$store.dispatch("suKienStore/getAll").then((res) => {
        this.suKien = res.data;
        if (res.data && res.data.length > 0) {
          const lastIndex = res.data.length - 1;
          this.suKien = res.data[lastIndex];
        }
      })
    },
    // async getThongTinSlider(){
    //   let promise =  this.$store.dispatch("sliderStore/getAll")
    //   return promise.then(resp => {
    //     if(resp.data == null){
    //       return []
    //     }else{
    //       if (resp.data != null )
    //       {
    //         this.listFileSilder = []
    //         this.listFileSilder = resp.data;
    //       }
    //     }
    //   })
    // },

    toggleMenu() {
      document.getElementById("topnav-menu-content").classList.toggle("show");
    },
    nextSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getNextPage());
    },
    prevSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getPreviousPage());
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },
    async getLinkWebsite(){
      await this.$store.dispatch("lienKetStore/getAll").then((res) => {
        this.lienketwebsite = res.data;
      })
    },
    async getDiemBan(){
      await this.$store.dispatch("diemBanStore/getAll").then((res) => {
        this.diemban = res.data;
      })
    },
    async getDiemBanLoTo(){
      await this.$store.dispatch("diemBanVeLoToStore/getAll").then((res) => {
        this.diembanloto = res.data;
      })
    },
    handleSelectChange() {
      if (this.selectedItem) {
        this.getId(this.selectedItem);
      }
    },
    getId(id){
      this.relink.push(process.env.VUE_APP_API_URL + 'filesminio/view/' + id)
      this.showPreview();
    },
    showPreview() {
      const preview = this.$imagePreview({
        initIndex: 0,
        images:this.relink,
        isEnableBlurBackground: false,
        isEnableLoopToggle: true,
        initViewMode: "contain",
        containScale: 1,
        shirnkAndEnlargeDeltaRatio: 0.2,
        wheelScrollDeltaRatio: 0.2,
        isEnableImagePageIndicator: true,
        maskBackgroundColor: "rgba(0,0,0,0.6)",
        zIndex: 4000,
        isEnableDownloadImage: true
      })
      this.relink=[]
    },
    async getVideo(){
      const params = {
        start: 0,
        limit: 3,
      };
      await this.$store.dispatch("videoStore/getFour", params).then((res) => {
        this.video = res.data.data;
      })
    },
    openNewTab(event) {
      const selectedValue = event.target.value;
      if (selectedValue !== "") {
        window.open(selectedValue, '_blank');
      }
    },
    async getKQXoSo() {
      await this.$store.dispatch("ketQuaStore/getbydate", this.modelKQ.date).then((res) => {
        if (res.data == null) {
          this.modelKQ = ketQuaXSModel.baseJson();
        } else {
          this.modelKQ = res.data;
          this.modelKQ.date = new Date(res.data.date);
          this.fnGetList();
        }
      });
    },
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    dateDisabled(ymd, date) {
        // Disable weekends (Sunday = `0`, Saturday = `6`) and
        // disable days that fall on the 13th of the month
        const weekday = date.getDay()
        const day = date.getDate()
        // Return `true` if the date should be disabled
        return weekday !== 1
      }
  },
};
</script>
<template>
  <Layout>
    <section class="section pt-0" id="home">
      <!-- <div class="row main">
        <div class="col-12">
          <div class="card">
            <div class="">
              <b-carousel id="carousel-fade" style="text-shadow: 0px 0px 2px #000">
                <b-carousel-slide v-for="slide in listFileSilder" :key="slide._id"
                                  :img-src= "apiView + slide.file.fileId">
                </b-carousel-slide>
              </b-carousel>
            </div>
          </div>
        </div>
      </div> -->
      <marquee class="event">{{ this.suKien.content }} </marquee>
      <div class="container" style="padding: 0 50px;">
        <b-row>
          <div class="wrap-main">
            <div class="row content">
              <div class="col-lg-8 col-md-6 col-sm-12 content-left">
                <div style="
                          font-size: 15px;
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          border-bottom: 2px solid #05912a;">
                          <router-link :to="{
                              path: `/tin-hoat-dong/655dc9425d377463faffe79c`,
                            }">
                          <div style="
                              background-color: #05912a;
                              width: 250px;
                              padding: 5px;
                              color: #fff;
                              ">
                                  Tin tức nổi bật
                              </div>
                          </router-link>
                      </div>
                <div class="row mt-2">
                  <div class="col-lg-8 col-md-12 col-sm-12">
                    <div class="row">
                      <b-carousel id="carousel-1"
                                  style="" controls :interval="10000"
                                  >
                        <b-carousel-slide v-for="(item, index) in tinnoibat" :key="index">
                          <div v-if="!item.fileImage">
                            <div class="float-left img-null">
                              <img
                                  src="@/assets/images/bg-login.jpg"
                                  alt="Không có ảnh"
                                  class="rounded-0 w-100"
                                  style="padding: 10px;"
                              >
                            </div>
                          </div>
                          <div v-else>
                            <b-card-img
                                :src="apiView + item.fileImage.fileId"
                                class="rounded-0 pb-3"
                            ></b-card-img>
                          </div>
                          <router-link
                              :to="{
                                      path: `/bang-tin/chi-tiet/${item._id}`,
                                      }"
                          >
                            <div class="carousel-name">
                              <a href="">
                                <h4 style="
                                  color: #0e0e0e;
                                  font-weight: bold;
                                  text-align: justify;
                                  display: -webkit-box !important;
                                  -webkit-line-clamp: 3;
                                  -webkit-box-orient: vertical;
                                  overflow: hidden;"
                                  >
                                  {{ item.name }}
                                </h4>
                                <p style="
                                      color: #000;
                                      text-align: justify;
                                      display: -webkit-box !important;
                                      -webkit-line-clamp: 5;
                                      -webkit-box-orient: vertical;
                                      overflow: hidden;"
                                >
                                  {{ item.moTa }}
                                </p>
                              </a>
                            </div>
                          </router-link>
                        </b-carousel-slide>
                      </b-carousel>
                    </div>
                  </div>
                  <div class="col-lg-4 col-md-4 list-tin">
                    <div class="list-news">
                      <div v-for="(item, index) in tinnoibat" :key="index">
                          <router-link
                              :to="{
                                  path: `/bang-tin/chi-tiet/${item._id}`,
                                  }"
                          >
                              <div class="pt-3" style="border-bottom: 1px solid #ccc;">
                              <a href="">
                                  <p style="
                                    color: #000;
                                    text-align: justify;
                                    display: -webkit-box !important;
                                    -webkit-line-clamp: 4;
                                    -webkit-box-orient: vertical;
                                    overflow: hidden;"
                                    >
                                      {{ item.name }}
                                    </p>
                              </a>
                              </div>
                          </router-link>
                      </div>
                    </div>
                    <!-- <a href="https://www.youtube.com/@xosokienthietdongthap" target="_blank">
                      <div class="live-yb" style="height: 150px; display: flex; align-items: center;">
                        <img src="@/assets/images/icon-yb.png" alt="" style="height: 70px; width: 70px;">
                        <span style="font-size: 16px; font-weight: bold; color: rgb(238 23 31); margin-left: 10px;">TRỰC TIẾP MỞ SỐ</span>
                      </div>
                    </a> -->
                  </div>
                </div>
              </div>
                <div class="col-lg-4 col-md-6 kqxs content-right">
                  <div class="row">
                    <div class="col-md-12" style="display: flex;">
                      <b-form-datepicker
                        :date-disabled-fn="dateDisabled"
                        :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                        :locale="locale"
                        id=""
                        v-model="modelKQ.date"
                        placeholder="Chọn ngày"
                        >
                      </b-form-datepicker>
                      <b-button type="button" variant="danger" @click="clearDate" style="height: 36px; margin-left: 5px !important;">
                        Xóa
                      </b-button>
                      <b-button type="button" variant="warning" v-on:click="getKQXoSo" style="height: 36px; margin-left: 5px !important;">
                        Tìm
                      </b-button>
                    </div>
                    <div class="">
                      <p class="" v-if="modelKQ.file">
                        <b-card-img :src="apiView + modelKQ.file.fileId" class="rounded-0" :height="550"
                        style="width: 100%; height: 550px; border-radius: 10px; border: 1px solid #05912a;
                        ">
                        </b-card-img>
                      </p>
                      <!-- <img src="@/assets/images/bg-ketqua.jpg" alt=""
                      style="width: 100%; height: 500px; border-radius: 10px; border: 1px solid #05912a;
                    box-shadow: #68ed8c 0px 2px 4px, #68ed8c 0px 7px 13px 2px, #68ed8c 0px -3px 0px inset;"> -->
                    </div>
                    <a href="https://www.youtube.com/@xosokienthietdongthap" target="_blank">
                      <div class="live-yb" style=" display: flex; align-items: center; justify-content: center;">
                        <img src="@/assets/images/icon-yb.png" alt="" style="height: 30px; width: 30px;">
                        <span style="font-size: 16px; font-weight: bold; color: rgb(238 23 31); margin-left: 10px;">TRỰC TIẾP MỞ SỐ</span>
                      </div>
                    </a>
                    <div class="col-md-12">
                      <div class="row">
                        <span style="font-weight: bold; color: #000; text-align: center; font-size: 15px !important; margin-top: 5px;">
                          Biên bản kết quả xổ số
                        </span>
                        <div class="col-md-3 col-sm-3 col-xs-6 bienban fs-13" v-for ="(item,index) in this.bienban" :key="index">
                          <router-link
                              :to="{
                                                path: `/bien-ban/chi-tiet/${item.file.fileId}`,
                                                }"
                              target="_blank"
                          >
                            {{ item.name }}
                          </router-link>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
            </div>
            <!--<div class="tructiep" style="height: 100px; display: none;">
              <div class="row">
                <div class="col-md-12 col">
                  <a href="https://www.youtube.com/@xosokienthietdongthap" target="_blank" style="display: flex; justify-content: center;">
                    <div class="live-yb" style="height: 100px; display: flex; align-items: center;">
                      <img src="@/assets/images/icon-yb.png" alt="" style="height: 70px; width: 70px;">
                      <span style="font-size: 16px; font-weight: bold; color: rgb(238 23 31); margin-left: 10px;">TRỰC TIẾP MỞ SỐ</span>
                    </div>
                  </a>
                </div>
              </div>
            </div>-->

            <!--LIST 3 TIN -->

            <div>
              <div class="row mt-2">

                <!--AN SINH XÃ HỘI-->
                  <div class="col-md-4">
                      <div style="
                          font-size: clamp(0.625rem, 0.4464rem + 0.5714vw, 0.875rem);
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          margin-top: 10px;
                          border-bottom: 2px solid #05912a;">
                          <router-link :to="{
                              path: `/tin-hoat-dong/65484358a202972a89722ac4`,
                            }">
                            <div style="
                                background-color: #05912a;
                                width: 200px;
                                padding: 5px;
                                color: #fff;
                                ">
                                    An sinh xã hội
                                </div>
                          </router-link>
                      </div>
                      <div class="row mt-2">
                          <div class="col-md-12 block-right">
                              <div v-for="(item, index) in this.tinansinhxahoi" :key="index">
                                <div v-if="index == 0">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                    <div class="row card-sukien mb-2">
                                        <div class="col-md-12 col-sm-12 img-news pd-2">
                                        <div v-if="!item.fileImage">
                                            <div class="float-left img-logo" style="text-align: center;">
                                            <img
                                                src="@/assets/images/logoXSKTDT.png"
                                                alt="Không có ảnh"
                                                class="rounded-0"
                                                style="height: 200px; padding: 10px; width: 150px; padding: 10px;"
                                            >
                                            </div>
                                        </div>
                                        <div v-else>
                                            <b-card-img
                                                :src="apiView + item.fileImage.fileId"
                                                class="rounded-0 img-res"
                                            ></b-card-img>
                                        </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 title-news mt-2">
                                        <a href="">
                                            <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                        </a>
                                        </div>
                                    </div>
                                  </router-link>
                                </div>
                                <div v-if="index !== 0" style="border-top: 1px solid #ccc;">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                    <div class="row card-sukien mb-2">
                                        <div class="col-md-12 col-sm-12 title-news mt-2">
                                          <a href="">
                                              <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                          </a>
                                        </div>
                                    </div>
                                  </router-link>
                                </div>
                              </div>
                          </div>
                      </div>
                  </div>

                <!--HOẠT ĐỘNG CÔNG TY-->
                  <div class="col-md-4">
                      <div style="
                          font-size: clamp(0.625rem, 0.4464rem + 0.5714vw, 0.875rem);
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          margin-top: 10px;
                          border-bottom: 2px solid #05912a;">
                          <router-link :to="{
                              path: `/tin-hoat-dong/6544b8f72a68b44e29eb5e9d`,
                            }">
                          <div style="
                              background-color: #05912a;
                              width: 200px;
                              padding: 5px;
                              color: #fff;
                              ">
                                  Hoạt động công ty
                              </div>
                          </router-link>
                      </div>
                      <div class="row mt-2">
                          <div class="col-md-12 block-right">
                              <div v-for="(item, index) in this.tinhoatdongcongty" :key="index">
                                <div v-if="index == 0">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                    <div class="row card-sukien mb-2">
                                        <div class="col-md-12 col-sm-12 img-news pd-2">
                                        <div v-if="!item.fileImage">
                                            <div class="float-left img-logo" style="text-align: center;">
                                            <img
                                                src="@/assets/images/logoXSKTDT.png"
                                                alt="Không có ảnh"
                                                class="rounded-0"
                                                style="height: 200px; padding: 10px; width: 150px; padding: 10px;"
                                            >
                                            </div>
                                        </div>
                                        <div v-else>
                                            <b-card-img
                                                :src="apiView + item.fileImage.fileId"
                                                class="rounded-0 img-res"
                                            ></b-card-img>
                                        </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 title-news mt-2">
                                        <a href="">
                                            <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                        </a>
                                        </div>
                                    </div>
                                  </router-link>
                                </div>
                                <div v-if="index !== 0" style="border-top: 1px solid #ccc;">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                    <div class="row card-sukien mb-2">
                                        <div class="col-md-12 col-sm-12 title-news mt-2">
                                          <a href="">
                                              <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                          </a>
                                        </div>
                                    </div>
                                  </router-link>
                                </div>
                              </div>
                          </div>
                      </div>
                  </div>

                <!--HOẠT ĐỘNG ĐOÀN THỂ-->
                <div class="col-md-4">
                      <div style="
                          font-size: clamp(0.625rem, 0.4464rem + 0.5714vw, 0.875rem);
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          margin-top: 10px;
                          border-bottom: 2px solid #05912a;">
                          <router-link :to="{
                              path: `/tin-hoat-dong/654842dfa202972a89722ac2`,
                            }">
                          <div style="
                              background-color: #05912a;
                              width: 250px;
                              padding: 5px;
                              color: #fff;
                              ">
                                  Hoạt động đảng, đoàn thể
                              </div>
                          </router-link>
                      </div>
                      <div class="row mt-2">
                          <div class="col-md-12 block-right">
                              <div v-for="(item, index) in this.tinhoatdongdoanthe" :key="index">
                                <div v-if="index == 0">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                    <div class="row card-sukien mb-2">
                                        <div class="col-md-12 col-sm-12 img-news pd-2">
                                        <div v-if="!item.fileImage">
                                            <div class="float-left img-logo" style="text-align: center;">
                                            <img
                                                src="@/assets/images/logoXSKTDT.png"
                                                alt="Không có ảnh"
                                                class="rounded-0"
                                                style="height: 200px; padding: 10px; width: 150px; padding: 10px;"
                                            >
                                            </div>
                                        </div>
                                        <div v-else>
                                            <b-card-img
                                                :src="apiView + item.fileImage.fileId"
                                                class="rounded-0 img-res"
                                            ></b-card-img>
                                        </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 title-news mt-2">
                                        <a href="">
                                            <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                        </a>
                                        </div>
                                    </div>
                                  </router-link>
                                </div>
                                <div v-if="index !== 0" style="border-top: 1px solid #ccc;">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                    <div class="row card-sukien mb-2">
                                        <div class="col-md-12 col-sm-12 title-news mt-2">
                                          <a href="">
                                              <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                          </a>
                                        </div>
                                    </div>
                                  </router-link>
                                </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
            </div>


            <!--VIDEO-->
            <div>
              <div class="row mt-4">
                <div class="col-md-8">
                  <div class="row">
                    <div class="col-md-12">
                        <div style="
                          font-size: 15px;
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          margin-top: 10px;
                          border-bottom: 2px solid #05912a;">
                          <div style="
                              background-color: #05912a;
                              width: 200px;
                              padding: 5px;
                              padding-left: 10px;
                              ">
                                  VIDEO HOẠT ĐỘNG
                          </div>
                        </div>
                    </div>
                    <div class="col-md-6 mt-2">
                      <div class="ratio ratio-16x9">
                        <iframe class="embed-responsive-item" :src="video[0]?.link"></iframe>
                      </div>
                      <div v-for="(item,index) in video" :key="index">
                        <div v-if="index==0" style="font-size: 14px; margin-top: 10px; text-align: center; color: #000;">{{ item.name }}</div>
                      </div>
                    </div>
                    <div class="col-md-6 mt-2">
                      <div class="row">
                        <div class="col-md-12" v-for="(item,index) in video" :key="index">
                          <div class="row" v-if="index !== 0">
                            <div class="col-md-6 video-sub">
                              <iframe class="embed-responsive-item" :src="item?.link" style="width: 100%;"></iframe>
                            </div>
                            <div class="col-md-6 mt-2 mb-2" style="color: #000;">
                              {{ item.name }}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="col-md-4 mt-2">
                  <div class="card-card border" style="height: 420px;">
                    <router-link :to="{
                      path: `/tin-hoat-dong/655c279fce5d0ab0f44b3edc`,
                    }">
                    <div class="card-title-cs bg-menu">
                      Thông báo
                    </div>
                    </router-link>
                    <div class="card-noti">
                      <div class="cate-list-noti" v-for ="(item,index) in listthongbao" :key="index">
                        <router-link
                          :to="{
                              path: `/bang-tin/chi-tiet/${item._id}`,
                              }"
                                                >
                            <i class="mdi mdi-bell" style="font-size: 20px;"></i>
                            {{ item.name }}
                        </router-link>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!--TIN DU LỊCH & TIN NỔI BẬT CỦA TỈNH-->
            <div>
              <div class="row mt-2">
                  <div class="col-md-8">
                      <div style="
                          font-size: 15px;
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          margin-top: 10px;
                          border-bottom: 2px solid #05912a;">
                          <router-link :to="{
                              path: `/tin-hoat-dong/655c2580f006435d39df0b15`,
                            }">
                            <div style="
                                background-color: #05912a;
                                width: 300px;
                                padding: 5px;
                                color: #fff;
                                ">
                                    Du lịch và đặc sản vùng miền
                            </div>
                          </router-link>
                      </div>
                      <div class="row mt-2">
                          <div class="col-md-12 block-right">
                              <div v-for="(item, index) in this.tindulich" :key="index">
                                  <router-link
                                      :to="{
                                          path: `/bang-tin/chi-tiet/${item._id}`,
                                          }"
                                  >
                                  <div class="row card-sukien mb-2">
                                      <div class="col-md-5 col-sm-5 img-news pd-2">
                                      <div v-if="!item.fileImage">
                                          <div class="float-left img-logo" style="text-align: center;">
                                          <img
                                              src="@/assets/images/logoXSKTDT.png"
                                              alt="Không có ảnh"
                                              class="rounded-0"
                                              style="height: 200px; padding: 10px; width: 150px; padding: 10px;"
                                          >
                                          </div>
                                      </div>
                                      <div v-else>
                                          <b-card-img
                                              :src="apiView + item.fileImage.fileId"
                                              class="rounded-0 img-res"
                                          ></b-card-img>
                                      </div>
                                      </div>
                                      <div class="col-md-7 col-sm-7 title-news">
                                      <a href="">
                                          <p style="color: #05912a; font-weight: bold; font-size: 15px;">{{ item.name }}  </p>
                                          <p style="
                                              color: #000;
                                              display: -webkit-box !important;
                                              -webkit-line-clamp: 2;
                                              -webkit-box-orient: vertical;
                                              overflow: hidden;"
                                              >
                                              {{ item.moTa }}
                                          </p>
                                      </a>
                                      </div>
                                  </div>
                                  </router-link>
                              </div>
                          </div>
                      </div>
                  </div>
                  <div class="col-md-4">
                    <div style="
                          font-size: 15px;
                          text-transform: uppercase;
                          color: #fff;
                          font-weight: bold;
                          margin-top: 10px;
                          border-bottom: 2px solid #05912a;">
                          <div style="
                              background-color: #05912a;
                              width: 200px;
                              padding: 5px;
                              ">
                                  Sự kiện nổi bật
                              </div>
                      </div>
                      <div class="img-kq" v-for ="(item,index) in lienket" :key="index">
                          <a target="_blank" :href="`${item?.link}`">
                            <img class="" :src="`${url}/${item.file.fileId}`" :alt="`${item.moTa}`">
                          </a>
                      </div>

                  </div>
              </div>
            </div>

            <!--HÌNH ẢNH VÉ SỐ-->
            <div>
              <div class="row">
                <div class="col-md-6" v-for ="(item,index) in imagehome" :key="index">
                  <div class="sample-kq" style="margin-top: 20px;">
                    <div style="padding-top: 10px;">
                      <h3 class="fw-bold" style="text-align: center; color: #05912a;">{{ item.name }}</h3>
                      <img :src="`${url}/${item.file.fileId}`">
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!--LIÊN KẾT-->
            <div class="mt-4">
              <div class="row">
                <div class="col-md-4">
                  <div class="card-card mb-4">
                    <div class="card-title-cs bg-menu">
                      Hệ thống đại lý vé số truyền thống
                    </div>
                    <select class="form-control input-sm" v-model="selectedItem" @change="handleSelectChange">
                        <option :value="null" disabled selected>— Chọn địa điểm —</option>
                        <!-- <option value="" disabled selected>Chọn một điểm mua...</option> -->
                        <option class="option" v-for="(item,index) in diemban" :key="index" :value="item.file.fileId">
                          <span @click="getId(item.file.fileId)"> {{ index + 1 }}. {{item.name}}</span>
                        </option>
                    </select>
                  </div>
                </div>

                <div class="col-md-4">
                  <div class="card-card mb-4">
                    <div class="card-title-cs bg-menu">
                      Hệ thống đại lý vé số lô tô - thủ công
                    </div>
                    <select class="form-control input-sm" v-model="selectedItem" @change="handleSelectChange">
                        <option :value="null" disabled selected>— Chọn địa điểm —</option>
                        <!-- <option value="" disabled selected>Chọn một điểm mua...</option> -->
                        <option class="option" v-for="(item,index) in diembanloto" :key="index" :value="item.file.fileId">
                          <span @click="getId(item.file.fileId)"> {{ index + 1 }}. {{item.name}}</span>
                        </option>
                    </select>
                  </div>
                </div>

                <div class="col-md-4">
                  <div class="card-card mb-4">
                    <div class="card-title-cs bg-menu">
                      Liên kết website
                    </div>
                    <select class="form-control input-sm" @change="openNewTab">
                        <option value="" disabled selected>— Liên kết website —</option>
                        <option v-for="(item, index) in lienketwebsite" :key="index" :value="item?.link">
                          {{ item.name }}
                        </option>
                    </select>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </b-row>

        <!-- THƯ VIỆN HÌNH ẢNH -->
        <div class="row mt-4">
          <div class="col-12 mb-2">
            <div style="
                  font-size: 15px;
                  text-transform: uppercase;
                  color: #fff;
                  font-weight: bold;
                  margin-top: 10px;
                  border-bottom: 2px solid #05912a;">
                  <div style="
                      background-color: #05912a;
                      width: 200px;
                      padding: 5px;
                      ">
                          Thư viện hình ảnh
                      </div>
              </div>
          </div>

          <div v-for ="(item,index) in loaiAnh" :key="index" class="col-md-4 col-sm-6 lib mt-4">
            <router-link
                :to="{
                    path: `/loai-anh/chi-tiet/${item._id}`,
                    }"
            >
              <div class="lib-img img-res">
                <img :src="`${url}/${item.file.fileId}`">
                <div class="decs">
                  <p>{{ item.name }}</p>
                </div>
              </div>
            </router-link>
          </div>

          <!-- <div v-for ="(item,index) in thuvienanh" :key="index" class="col-md-3 col-sm-6 lib mt-4">
            <div class="lib-img">
              <img :src="`${url}/${item.file.fileId}`">
              <div class="decs">
                <p>{{ item.name }}</p>
              </div>
            </div>
          </div> -->
        </div>

      </div>
    </section>
  </Layout>
</template>


<style>

.img-null{
  height: 400px;
}
.img-null img{
  height: 100%;
}
.main{
  margin-top: 226px;
}

@media only screen and (max-width: 992px) {
  .main{
    margin-top: 176px;
  }
  .tructiep{
    display: block !important;
  }
}

@media only screen and (max-width: 900px) {
  .main{
    margin-top: 146px;
  }
}

@media only screen and (max-width: 800px) {
  .main{
    margin-top: 126px;
  }
}
@media only screen and (max-width: 700px) {
  .main{
    margin-top: 116px;
  }
}
@media only screen and (max-width: 600px) {
  .main{
    margin-top: 106px;
  }
}
@media only screen and (max-width: 500px) {
  .main{
    margin-top: 86px;
  }
}

.bienban{
  text-align: center;
}
.dropdown-menu.show{
  width: 100% !important;
}
.b-calendar-inner{
  width: 100% !important;
}
.content{
  display: flex;
  justify-content: center;
  max-height: 700px;
}
@media only screen and (max-width: 768px) {
  .content .content-left{
    margin-top: 20px;
    order: 2;
  }
  .content .content-right{
    order: 1;
  }
}
.list-news{
  max-height: 650px;
  /* overflow: scroll;
  overflow-x: hidden; */
}
.list-news:hover a{
  color: #ED0678 !important;
}
.fs-20{
  font-size: 20px;
}

.pd-10{
  padding: 10px;
}

.td-stt {
  width: 50px;
}

.td-content {
  width: 50px;
}

.card{
  margin-bottom: 0px !important;
}

.mt-175{
  margin-top: 175px;
}

.wrap-one{
  padding-top: 20px;
  float: left;
}

/* .wrap-main{
  padding-top: 20px;
} */

.link-pdf{
  margin-top: 5px;
}

.img-kq{
  margin: 10px 0;
}
.img-kq img{
  width: 100%;
}

.card-title-cs{
  padding: 10px;
  color: #fff;
  text-align: center;
  margin: 0;
  text-transform: uppercase;
  font-weight: bold;
}


.border{
  border: 1px solid #ccc !important;
}

.sample-kq img{
  width: 100%;
  height: 300px;
  border-radius: 10px;
}

.sample-kq{
  margin-bottom: 10px;
  border-radius: 10px;
  box-shadow: rgba(17, 17, 26, 0.1) 0px 0px 5px;
  min-height: 300px;
}

.card-title-news{
  text-align: center;
}

.news{
  padding: 10px;
}

.block-post{
  text-align: justify;
}

.block-left{
  border-right: 1px solid #ccc;
}

.lib-img img{
  border-radius: 10px;
  width: 100%;
}
.lib-img{
  display: flex;
  justify-content: center;

}


.lib img{
	transform: scale(1);
	transition: .3s ease-in-out;
}

.lib:hover img{
  -webkit-transform: scale(1.1);
	transform: scale(1.1);
  opacity: 0.7;
}

.decs{
  position: absolute;
  color: #fff;
  font-size: 13px;
  display: flex;
  font-weight: bold;
  top:-25px;
}

.decs p{
  padding: 5px;
  text-align: center;
  transform: translateY(40px);
	transition: 0.5s;
	opacity: 0;
  z-index: 1;
  background-color: #05912a;
  border-radius: 5px;
}
.lib:hover .decs p{
  transform: translateY(0px);
	opacity: 1;
}


.title-search{
  width: 100%;
  text-align: center;
}

.title-search h3{
  color: #ec232b;
}

/* CARD */
article {
  --img-scale: 1.001;
  --title-color: black;
  --link-icon-translate: -20px;
  --link-icon-opacity: 0;
  position: relative;
  border-radius: 16px;
  box-shadow: none;
  background: #fff;
  transform-origin: center;
  transition: all 0.4s ease-in-out;
  overflow: hidden;
  padding-left: 0 !important;
  padding-right: 0 !important;
}

article a::after {
  position: absolute;
  inset-block: 0;
  inset-inline: 0;
  cursor: pointer;
  content: "";
}

/* basic article elements styling */
article h2 {
  margin: 0 0 18px 0;
  font-family: "Bebas Neue", cursive;
  font-size: 1.9rem;
  letter-spacing: 0.06em;
  color: var(--title-color);
  transition: color 0.3s ease-out;
}

figure {
  margin: 0;
  padding: 0;
  aspect-ratio: 16 / 9;
  overflow: hidden;
}

article img {
  max-width: 100%;
  transform-origin: center;
  transform: scale(var(--img-scale));
  transition: transform 0.4s ease-in-out;
}

.article-body {
  padding: 20px;
}

article a {
  display: inline-flex;
  align-items: center;
  text-decoration: none;
  color: #28666e;
}

article a:focus {
  outline: 1px dotted #28666e;
}

article a .icon {
  min-width: 24px;
  width: 24px;
  height: 24px;
  margin-left: 5px;
  transform: translateX(var(--link-icon-translate));
  opacity: var(--link-icon-opacity);
  transition: all 0.3s;
}

/* using the has() relational pseudo selector to update our custom properties */
article:has(:hover, :focus) {
  --img-scale: 1.1;
  --title-color: #28666e;
  --link-icon-translate: 0;
  --link-icon-opacity: 1;
  box-shadow: rgba(0, 0, 0, 0.16) 0px 10px 36px 0px, rgba(0, 0, 0, 0.06) 0px 0px 0px 1px;
}


/************************
Generic layout (demo looks)
**************************/

*,
*::before,
*::after {
  box-sizing: border-box;
}
/* .articles {
  display: grid;
  max-width: 1200px;
  margin-inline: auto;
  padding-inline: 24px;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 24px;
} */

@media screen and (max-width: 992px) {
  article {
    container: card/inline-size;
    margin-bottom: 10px;
  }
  .article-body h2 {
    font-size: 18px !important;
  }
  .list-tin{
    display: none !important;
  }
}

@media screen and (min-width: 768px) {
  .content{
    height: 100% !important;
  }
}

@media screen and (max-width: 768px) {
  .content{
    max-height: 100% !important;
  }
  .video-sub{
    height: 300px !important;
  }
  .video-sub iframe{
    height: 100%;
  }
  #carousel-1 .carousel-inner{
    height: 700px !important;
  }
}

/* @media screen and (max-width: 960px) {
  article {
    container: card/inline-size;
  }
  .article-body p {
    display: none;
  }
} */


@container card (min-width: 380px) {
  .article-wrapper {
    display: grid;
    grid-template-columns: 100px 1fr;
    gap: 16px;
  }
  .article-body {
    padding-left: 0;
  }
  figure {
    width: 100%;
    height: 100%;
    overflow: hidden;
  }
  figure img {
    height: 100%;
    aspect-ratio: 1;
    object-fit: cover;
  }
}

.sr-only:not(:focus):not(:active) {
  clip: rect(0 0 0 0);
  clip-path: inset(50%);
  height: 1px;
  overflow: hidden;
  position: absolute;
  white-space: nowrap;
  width: 1px;
}

.img-lienket img{
  width: 100%;
  height: 150px;
}

.img-tintuc{
  border-radius: 20px !important;
}

.title-news{
  display: flex;
  align-items: center;
}

.title-news a p{
  margin-bottom: 0px;
  text-align: justify;
}
.news-title{
  text-align: justify;
}

.card-noti{
  height: 379px;
  overflow: scroll;
  overflow-x: hidden !important;
}

.cate-list-noti{
  padding: 10px;
}

.cate-list-noti ul li{
  list-style: none;
  padding: 10px 0 10px 0;
}

@media (max-width: 768px){
    .lib{
      margin-bottom: 10px;
    }
}

#carousel-1 .carousel-inner {
  height: 650px;
}
#carousel-1 .carousel-caption {
  bottom:-200px
}
#carousel-1 .carousel-inner img {
  object-fit: cover;
}
.carousel-control-next, .carousel-control-prev{
  height: 450px;
}
@media screen and (max-width: 576px) {

  #carousel-1 .carousel-inner{
    height: 600px !important;
  }
  .carousel-control-next, .carousel-control-prev{
    height: 350px;
  }
}
@media screen and (max-width: 430px) {

  #carousel-1 .carousel-inner{
    height: 450px !important;
  }
}
.carousel-item{
  position: absolute !important;
}

.carousel-caption{
  position:sticky;
  left: 0 !important;
  right: 0 !important;
}

.option{
  border-radius: 0 !important;
}

</style>
