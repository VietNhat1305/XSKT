<script>
import {Carousel, Slide} from "vue-carousel";
import Layout from "@/pages/congdan/layout3";
import {required} from "vuelidate/lib/validators";
import Multiselect from "vue-multiselect";
import {notifyModel} from "@/models/notifyModel";
import vue2Dropzone from "vue2-dropzone";
// import the component
import Treeselect from "@riophae/vue-treeselect";
// import the styles
import "@riophae/vue-treeselect/dist/vue-treeselect.css";
import Vue from "vue";
import {mapState} from "vuex";
import moment from "moment";
import 'vue-slick-carousel/dist/vue-slick-carousel.css'
import 'vue-slick-carousel/dist/vue-slick-carousel-theme.css'
import flatpickr from "flatpickr";
import flatPickr from "vue-flatpickr-component";

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
      relink:[],
      video:[],
      bienban:[],
      tinnoibat: [],
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
    this.getThongTinSlider();
    this.getSuKien();
    this.getLink();
    this.getTinTrangChu();
    this.getThongBao();
    this.getThuVienAnh();
    this.getKQXoSo();
    this.getImageHome();
    this.getLinkWebsite();
    this.getDiemBan();
    this.getVideo();
    this.getBienBan();
    this.getTinNoiBat();
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  mounted() {
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
    async getTinNoiBat(){
      const params = {
        start: this.currentPage,
        limit: this.perPage,
        sortDesc: true,
        menuId: "655dc9425d377463faffe79c",
      };
      await this.$store.dispatch("contentStore/getByMenuId", params).then((res) => {
        console.log("TIN NOI BAT: ", res.data.data);
        this.tinnoibat = res.data.data;
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
      const params = {
        start: 0,
        limit: 4,
      };
      await this.$store.dispatch("thuvienanhStore/getFour", params).then((res) => {
        this.thuvienanh = res.data.data;
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
      await this.$store.dispatch("linkStore/getAll").then((res) => {
        this.lienket = res.data;
      })
    },
    async getSuKien(){
      await this.$store.dispatch("suKienStore/getAll").then((res) => {
       // console.log("SU KIEN: ", res.data)
        this.suKien = res.data;
        if (res.data && res.data.length > 0) {
      // Lấy phần tử cuối cùng của mảng
          const lastIndex = res.data.length - 1;
          this.suKien = res.data[lastIndex];
        }
      })
    },
    async getThongTinSlider(){
      let promise =  this.$store.dispatch("sliderStore/getAll")
      return promise.then(resp => {
        if(resp.data == null){
          return []
        }else{
          if (resp.data != null )
          {
            this.listFileSilder = []
            this.listFileSilder = resp.data;
        //    console.log('LOG SLIDER', this.url + this.listFileSilder[0].file.fileId)
          }
        }
      })
    },

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
    async getKQXoSo(){

      await this.$store.dispatch("ketQuaStore/getbydate", null).then((res) => {
        this.ketquaxoso = res.data;
      });
    },
    async getLinkWebsite(){
      await this.$store.dispatch("lienKetStore/getAll").then((res) => {
        this.lienketwebsite = res.data;
        console.log('log website', this.lienketwebsite)
      })
    },
    async getDiemBan(){
      await this.$store.dispatch("diemBanStore/getAll").then((res) => {
        this.diemban = res.data;
        // res.data.forEach(elements => {
        //     elements.diemban.forEach(
        //         src => this.relink.push(process.env.VUE_APP_API_URL + 'filesminio/view/' + src.fileId))
        // });
        //this.diemban.forEach(elements => this.relink.push(process.env.VUE_APP_API_URL + 'filesminio/view/' + elements.file.fileId))
        console.log('log diem ban', this.diemban)
        console.log('log relink', this.relink)
      })
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
      await this.$store.dispatch("videoStore/getAll").then((res) => {
        this.video = res.data;
        console.log('log video', this.video)
      })
    },
    openNewTab(event) {
      const selectedValue = event.target.value;
      if (selectedValue !== "") {
        window.open(selectedValue, '_blank');
      }
    },
  },
};
</script>
<template>
  <Layout>
    <section class="section pt-4" id="home">
      <div class="row" style="margin-top: 45px;">
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
      </div>
      <marquee class="event" style="margin-top: 0px !important;">{{ this.suKien.content }} </marquee>
      <div class="container" style="padding: 0 50px;">
        <b-row>
          <div class="col-md-9"></div>
          <div class="col-md-3 mt-2" style="padding-right: 40px;">
            <input type="date" min="2000-01-01" class="date">
          </div>
          <!-- <div class="wrap-one">
            <b-col md="12">
              <div class="col-lg-12">
                <h1 style="font-weight: bold; color: #000;">KQXS ĐỒNG THÁP THỨ HAI HÀNG TUẦN</h1>
              </div>
              <div class="col-lg-4">
              </div>
            </b-col>
          </div> -->
          <div class="wrap-main">
            <div class="row content">
                <!-- <div class="col-md-3">
                    <div class="">
                      <p class="" v-if="ketquaxoso">
                        <b-card-img :src="apiView + ketquaxoso.file.fileId" class="rounded-0" :height="600"
                        style="padding: 0 10px;">
                        </b-card-img>
                      </p>
                    </div>
                </div> -->
                <div class="col-md-6 col-sm-12 row">
                  <div class="col-md-12">
                    <b-carousel id="carousel-1"
                                style="" controls :interval="4000"
                                >
                      <b-carousel-slide v-for="(item, index) in tinnoibat" :key="index"
                        :img-src= "apiView + item.fileImage.fileId">
                        <router-link
                            :to="{
                                    path: `/bang-tin/chi-tiet/${item._id}`,
                                    }"
                        >
                          <div class="carousel-name">
                            <a href="">
                              <h4 style="color: #0e0e0e; font-weight: bold;">{{ item.name }}  </h4>
                              <p style="
                                    color: #000;
                                    display: -webkit-box !important;
                                    -webkit-line-clamp: 3;
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
                <div class="col-md-3 list-news">
                  <div v-for="(item, index) in tinnoibat" :key="index">
                          <router-link
                              :to="{
                                  path: `/bang-tin/chi-tiet/${item._id}`,
                                  }"
                          >
                              <div class="pt-3" style="border-bottom: 1px solid #ccc;">
                              <a href="">
                                  <p style="color: #000;">{{ item.name }} </p>
                              </a>
                              </div>
                          </router-link>
                      </div>
                </div>
                <div class="col-md-3">
                    <div class="">
                      <img src="@/assets/images/bg-ketqua2.jpg" alt=""
                    style="width: 100%; height: 600px; border-radius: 10px; border: 1px solid #ED0678;
                    box-shadow: #e674adfb 0px 2px 4px, #e674adfb 0px 7px 13px 2px, #e674adfb 0px -3px 0px inset;">
                      <!-- <p class="" v-if="ketquaxoso">
                        <b-card-img :src="apiView + ketquaxoso.file.fileId" class="rounded-0" :height="600"
                        style="padding: 0 10px;">
                        </b-card-img>
                      </p> -->
                    </div>
                </div>
            </div>

            <!-- TIN TIN TIN -->
            <div class="row block-post" v-for="(item, index) in listtin" :key="index">
                <div v-if="index === 0">
                    <div style="
                        font-size: 15px;
                        text-transform: uppercase;
                        color: #fff;
                        font-weight: bold;
                        margin-top: 10px;
                        border-bottom: 2px solid #ec008b;">
                        <div style="
                            background-color: #ec008b;
                             width: 30%;
                             padding: 5px;
                             ">
                                {{ item.name }}
                            </div>
                    </div>
                    <div class="row mt-2">
                        <div class="col-md-12 card-list-news">
                            <div class="row">
                                <div class="col-md-3 mb-2" v-for="(items, index) in item.danhSach" :key="index">
                                    <div>
                                        <router-link
                                            :to="{
                                                path: `/bang-tin/chi-tiet/${items._id}`,
                                                }"
                                        >
                                            <div class="img-news pd-2">
                                                <div v-if="!items.fileImage">
                                                <div class="float-left img-logo" style="text-align: center;">
                                                    <img
                                                        src="@/assets/images/logoXSKTDT.png"
                                                        alt="Không có ảnh"
                                                        class="rounded-0"
                                                        style="height: 300px; padding: 10px;"
                                                    >
                                                </div>
                                                </div>
                                                <div v-else>
                                                    <b-card-img
                                                        :src="apiView + items.fileImage.fileId"
                                                        class="rounded-0"
                                                        :height="300"
                                                        :width="250"
                                                    ></b-card-img>
                                                </div>
                                            </div>
                                            <div class="title-news pt-3">
                                            <a href="">
                                                <p style="color: #bb1a78; font-weight: bold; font-size: 15px;">{{ items.name }}  </p>
                                                <p style="
                                                color: #000;
                                                display: -webkit-box !important;
                                                -webkit-line-clamp: 2;
                                                -webkit-box-orient: vertical;
                                                overflow: hidden;"
                                                >
                                                {{ items.moTa }}
                                                </p>
                                            </a>
                                            </div>
                                        </router-link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div v-if="index !== 0">
                    <div class="row mt-2">
                        <div class="col-md-8">
                            <div style="
                                font-size: 15px;
                                text-transform: uppercase;
                                color: #fff;
                                font-weight: bold;
                                margin-top: 10px;
                                border-bottom: 2px solid #ec008b;">
                                <div style="
                                    background-color: #ec008b;
                                    width: 30%;
                                    padding: 5px;
                                    ">
                                        {{ item.name }}
                                    </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-12 block-right">
                                    <div v-for="(items, index) in item.danhSach" :key="index">
                                        <router-link
                                            :to="{
                                                path: `/bang-tin/chi-tiet/${items._id}`,
                                                }"
                                        >
                                        <div class="row card-sukien mb-2">
                                            <div class="col-md-5 col-sm-5 img-news pd-2">
                                            <div v-if="!items.fileImage">
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
                                                    :src="apiView + items.fileImage.fileId"
                                                    class="rounded-0"
                                                    :height="200"
                                                    :width="250"
                                                ></b-card-img>
                                            </div>
                                            </div>
                                            <div class="col-md-7 col-sm-7 title-news">
                                            <a href="">
                                                <p style="color: #bb1a78; font-weight: bold; font-size: 15px;">{{ items.name }}  </p>
                                                <p style="
                                                    color: #000;
                                                    display: -webkit-box !important;
                                                    -webkit-line-clamp: 2;
                                                    -webkit-box-orient: vertical;
                                                    overflow: hidden;"
                                                    >
                                                    {{ items.moTa }}
                                                </p>
                                            </a>
                                            </div>
                                        </div>
                                        </router-link>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 mt-3">
                            <div class="card-card border mb-4">
                            <div class="card-title-cs bg-menu">
                                Thông báo
                            </div>
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

                            <div class="card-card mb-4">
                            <div class="card-title-cs bg-menu">
                                Liên kết
                            </div>
                            <select class="form-control input-sm" @change="openNewTab">
                                <option value="">— Liên kết website —</option>
                                <option v-for="(item, index) in lienketwebsite" :key="index" :value="item?.link">
                                    {{ item.name }}
                                </option>
                            </select>
                            </div>
                            <div class="img-kq" v-for ="(item,index) in lienket" :key="index">
                                <a target="_blank" :href="`${item?.link}`"><img :src="`${url}/${item.file.fileId}`" :alt="`${item.moTa}`"></a>
                            </div>

                        </div>
                    </div>
                </div>

            </div>

            <!-- VIDEO -->
            <div class="row mt-4">
                <div class="col-md-12">
                    <div style="
                    font-size: 15px;
                    text-transform: uppercase;
                    color: #fff;
                    font-weight: bold;
                    margin-top: 10px;
                    border-bottom: 2px solid #ec008b;">
                    <div style="
                        background-color: #ec008b;
                        width: 10%;
                        padding: 5px;
                        padding-left: 10px;
                        ">
                            VIDEO
                        </div>
                </div>
                </div>
                <div class="col-md-6 mt-2">
                  <div class="ratio ratio-16x9">
                    <iframe class="embed-responsive-item" :src="video[0]?.link"></iframe>
                  </div>
                </div>
                <div class="col-md-6 mt-2">
                  <div class="row">
                    <div class="col-md-12" v-for="(item,index) in video" :key="index">
                      <div class="row" style="height: 200px;">
                        <div class="col-md-6">
                          <iframe class="embed-responsive-item" :src="item?.link" style="width: 100%; height: 190px;"></iframe>
                        </div>
                        <div class="col-md-6" style="color: #000;">
                          {{ item.name }}
                        </div>
                      </div>
                    </div>
                  </div>
                  <!-- <div class="ratio ratio-16x9 mb-2" v-for="(item,index) in video" :key="index">
                    <iframe class="embed-responsive-item" :src="item?.link"></iframe>
                  </div> -->
                </div>
            </div>
          </div>
          <div class="wrap-main">
            <div class="wrap-main">
            <div class="row">
              <!-- MAIN LEFT -->
              <div class="col-md-6">
                <div class="row">
                    <div class="col-md-12">
                        <div class="sample">
                            <div>
                                <h3 class="fw-bold name-bienban" style="color: #ec008b; text-align: center; padding-top: 10px;">Biên bản kết quả xổ số</h3>
                            </div>
                            <div class="row" style="text-align: center; padding-bottom: 10px;">
                                <div class="col-md-3 col-sm-3 col-xs-3 bienban" v-for ="(item,index) in this.bienban" :key="index" style="margin-top: 10px;">
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
              <div class="col-md-6">
                  <div class="sample">
                      <div class="mb-3" style="padding-top: 20px;">
                          <h3 class="fw-bold color" style="text-align: center;">Các điểm mua Xổ số Lô tô thủ công</h3>
                          <div class="row" style="padding: 10px; text-align: center;">
                          <div class="col-md-4 col-sm-4" v-for="(item,index) in diemban" :key="index" style="color: #b30d0a; font-size: 15px;">
                              <p type="button" @click="getId(item.file.fileId)"> {{ index + 1 }}. {{item.name}}</p>
                          </div>
                          </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-12">
                <div class="sample">
                    <div style="padding: 10px;">
                        <h3 class="fw-bold color" style="text-align: center;">Nơi lãnh thưởng</h3>
                        <p class="fs-15">+ Số 01, đường Duy Tân, phường Mỹ Phú, TP Cao Lãnh, tỉnh Đồng Tháp. ĐT: 02773.861746</p>
                        <p class="fs-15">+ Số 03, đường Phạm Đình Toái, phường Võ Thị Sáu, quận 3, Tp.Hồ Chí Minh. ĐT: 0286.6823966</p>
                    </div>
                </div>
              </div>

              <div class="col-md-6" v-for ="(item,index) in imagehome" :key="index">
                <div class="sample" style="margin-top: 20px;">
                  <div style="padding-top: 10px;">
                    <h3 class="fw-bold color" style="text-align: center;">{{ item.name }}</h3>
                    <img style="height: 330px !important;" :src="`${url}/${item.file.fileId}`">
                  </div>
                </div>
              </div>

            </div>
          </div>
          </div>
        </b-row>

        <!-- THƯ VIỆN HÌNH ẢNH -->
        <div class="row mt-4">
          <div class="col-12 mb-2">
            <div class="cs-title-box">
              <div class="cs-title py-2">
                <a href="">
                  <i class="mdi mdi-star ic-item"></i>
                  <span class="fs-20" style="color: #fff;">Hình ảnh</span>
                </a>
              </div>
            </div>
          </div>

          <div v-for ="(item,index) in thuvienanh" :key="index" class="col-md-3 col-sm-6 lib mt-4">
            <div class="lib-img">
              <img :src="`${url}/${item.file.fileId}`">
              <div class="decs">
                <p>{{ item.name }}</p>
              </div>
            </div>
          </div>
        </div>

      </div>
    </section>
  </Layout>
</template>


<style>
.content{
  max-height: 600px;
}
.list-news{
  height: 600px;
  overflow: scroll;
  overflow-x: hidden;
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

.wrap-main{
  padding-top: 20px;
}

.link-pdf{
  margin-top: 5px;
}

.img-kq{
  margin: 10px 0;
}
.img-kq img{
  width: 100%;
}

.card-card{
  margin-top: 10px;
}

.card-title-cs{
  padding: 10px;
  color: #fff;
  text-align: center;
  margin: 0;
  text-transform: uppercase;
}


.border{
  border: 1px solid #ccc !important;
}

.sample img{
  width: 100%;
  height: 450px;
}

.sample{
  margin-bottom: 10px;
  border-radius: 10px;
  box-shadow: rgba(17, 17, 26, 0.1) 0px 0px 5px;
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
  height: 200px;
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
  background-color: #ec232b;
  border-radius: 10px;
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
.news-title{
  text-align: justify;
}

.card-noti{
  max-height: 300px;
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

#carousel-3 .carousel-inner {
  height: 400px;
}
#carousel-3 .carousel-inner img {
  height: 400px !important;
}

#carousel-3 .carousel-inner:hover img {
  opacity: 0.1;
}
.carousel-caption{
  left: 0 !important;
  right: 0 !important;
  bottom: 0;
  padding-bottom: 0px !important;
}

</style>
