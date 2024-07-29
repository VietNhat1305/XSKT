<script>
import Layout from "@/pages/congdan/layout";
import {latLng} from "leaflet";
import {pagingModel} from "@/models/pagingModel";
import Multiselect from "vue-multiselect";


/**
 * Crypto ICO-landing page
 */
export default {
  components: {
    Layout,
    Multiselect
  },
  data() {
    return {
      data: [],
      fields: [
        { key: 'STT',
          label: 'STT',
          class: 'td-stt',
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'},
        {
          key: "ngayXuatBanShow",
          label: "Ngày giờ đăng",
          class: 'td-skh',
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        // {
        //   key: "ngayKyShow",
        //   label: "Ngày ban hành",
        //   class: 'td-skh',
        //   thStyle: "text-align:center",
        //   thClass: 'hidden-sortable'
        // },
        {
          key: "soKiHieu",
          label: "Số ký hiệu",
          class: 'td-skh',
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          class: '',
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "files",
          label: "Tải văn bản",
          class: 'td-download',
          thClass: 'hidden-sortable',
          thStyle: {width: '100px', minWidth: '60px'},
        },

      ],
      filter: null,
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5 ,10, 25, 50, 100],
      sortBy: "age",
      sortDesc: false,
      filterOn: [],
      model:[],
      menu : null,
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
      gioithieu: null,
      pagination: pagingModel.baseJson(),
      apiView: `${process.env.VUE_APP_API_URL}filesminio/dowload`,
      listDanhMuc : [],
      selectedYear: null,
      itemFilter: {
        nam : null,
      },
      nam:null
    };
  },
  watch: {
    $route(to, from) {
      this.gioithieu = this.$route.params._id;
      this.$refs.tblList.refresh();
      this.getMenu()
    //  console.log("LOG CREATED LIEN HE ",this.$route.path)
    },
    "nam"(value) {
      console.log(value)
      this.itemFilter.nam = value.id;
      this.$refs.tblList.refresh();
      console.log('item fill',this.itemFilter.nam)
    }

  },
  created() {
    this.getFiles();
    this.getMenu();
    this.GetDanhSach();
    this.getCurrentYear()
    window.addEventListener("scroll", this.windowScroll);
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  computed: {
    years() {
      const year = []
      const current = (new Date().getFullYear()) + 1
      for (let i = current-5 ; i < current; i++) {
        year.push({id: i, name: `Năm ${i}`})
      }
      return year
    },
  },
  mounted() {
    // this.years();
  },
  methods: {
    async GetDanhSach(){
      await this.$store.dispatch("menuCongDanStore/getDanhMuc").then((res) => {
        this.listDanhMuc = res.data;
        //    console.log("LOG DANH MUC : ", this.listDanhMuc)
      })
    },
    async getCurrentYear() {
      const current = (new Date().getFullYear());
      this.nam = {id: current, name: `Năm ${current}`};
      this.itemFilter.nam = current;
    },
    async getMenu(){
      let menuStorage = localStorage.getItem("flatten-menu");
      if (menuStorage) {
        this.menu =  JSON.parse(menuStorage).find(item => item.link ===window.location.pathname);
        if (this.menu == null)
        {
          this.menu = JSON.parse(menuStorage).find(item => item.id === this.$route.params.id);
        }
      }
    },
    async getFiles(){
      await this.$store.dispatch("contentStore/getByMenuId",{
        "id": this.$route.params.id
      } ).then((res) => {
        this.model = res.data;
      })
    },

    getColorWithExtFile(ext) {
      if (ext == '.docx' || ext == '.doc')
        return 'icon-word';
      else if (ext == '.xlsx' || ext == '.xls')
        return 'icon-excel';
      else if (ext == '.pdf')
        return 'icon-pdf';
      else return 'text-danger';
    },

    getIconWithExtFile(ext) {
      if (ext == '.docx' || ext == '.doc')
        return 'mdi mdi-microsoft-word';
      else if (ext == '.xlsx' || ext == '.xls')
        return 'mdi mdi-microsoft-excel';
      else if (ext == '.pdf')
        return 'mdi mdi-file-pdf-outline';
      else if (ext == '.pptx')
        return 'fas fa-file-powerpoint';
      else return 'mdi-file-clock-outline';
    },

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
    reloadData() {
      // Gọi lại hàm myProvider để tải dữ liệu mới dựa trên năm được chọn
      this.$refs.myProvider.call();
    },

    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        content: this.filter,
        menuId: this.$route.params.id,
        year: this.itemFilter.nam,
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("contentStore/getPagingParams", params
      )
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
        })
      } finally {
        this.loading = false
      }
    },
  },

};
</script>

<template>
  <Layout>
    <section class="section p-2 bg-white" id="about">
        <div class="row align-items-center">
          <div>
            <div class="row">
              <div class="col-12">
                <div class="row">
                  <div class="col-lg-12">
                    <div class="container">
                      <div class="row">
                        <div class="col-lg-9 mb-3">
                          <div class="row">
                            <div class="col-lg-8 col-md-8">
                              <div class="cs-title-box">
                                <div class="cs-title py-2 mb-4" style="padding-top: 7px !important;">
                                  <a href="">
                                    <i class="mdi mdi-star ic-item"></i>
                                    <span style="color: #fff; font-size: 16px;" v-if="this.menu != null && this.menu.name != null">{{this.menu.name }}</span>
                                  </a>
                                </div>
                              </div>
                            </div>
                            <div class="col-lg-4 col-md-4 search-box">
                              <multiselect
                                  v-model="nam"
                                  :options="years"
                                  track-by="id"
                                  class="helo"
                                  label="name"
                                  placeholder="Chọn năm... "
                                  selectLabel="Nhấn vào để chọn"
                                  deselectLabel="Nhấn vào để xóa"
                              ></multiselect>
                                <!--<div class="me-2 mb-2 d-inline-block">
                                    <div class="position-relative">
                                        <input
                                            v-model="filter"
                                            type="text"
                                            class="form-control"
                                            placeholder="Tìm kiếm ..."
                                        />
                                        <i class="bx bx-search-alt search-icon"></i>
                                    </div>
                                </div>-->
                            </div>
                            <div class="col-sm-12 col-md-6">
                              <div
                                  class="col-sm-12 d-flex justify-content-left align-items-center"
                              >
                                <div
                                    id="tickets-table_length"
                                    class="dataTables_length m-1"
                                    style="
                                  display: flex;
                                  justify-content: left;
                                  align-items: center;
                                "
                                >
                                  <div class="me-1" >Hiển thị </div>
                                  <b-form-select
                                      class="form-select form-select-sm"
                                      v-model="perPage"
                                      size="sm"
                                      :options="pageOptions"
                                      style="width: 70px"
                                  ></b-form-select
                                  >&nbsp;
                                  <div style="width: 100px"> dòng </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div class="table-responsive mb-0 table-height">
                            <b-table
                            class="datatables"
                                  :items="myProvider"
                                  :fields="fields"
                                  striped
                                  bordered
                                  responsive="sm"
                                  :per-page="perPage"
                                  :current-page="currentPage"
                                  :sort-by.sync="sortBy"
                                  :sort-desc.sync="sortDesc"
                                  :filter="filter"
                                  :filter-included-fields="filterOn"
                                  ref="tblList"
                                  primary-key="id"
                            >
                              <template v-slot:cell(STT)="data">
                                {{ data.index + ((currentPage-1)*perPage) + 1  }}
                              </template>
                              <template v-slot:cell(soKiHieu)="data">
                                <span>{{ data.item.soKiHieu }}</span>
                                <div class="row ngayKi">
                                  <span>Ngày ký: {{ data.item.ngayKyShow }}</span>
                                </div>
                              </template>
                              <template v-slot:cell(files)="data">

                                  <div v-for ="(item,index) in data.item.files" :key="index">
                                    <router-link
                                              :to="{
                                                path: `/xem-chi-tiet/${item.fileId}`,
                                              }"
                                                target="_blank"
                                                class="fw-medium"
                                    >
                                            <i class="mdi mdi-cloud-download" style="font-size: 24px; color: #B50027;"></i>
                                    </router-link>

                                    <!-- <a
                                        :href="`${url}/${item.fileId}`"
                                        class="text-dark fw-medium d-flex justify-content-center align-items-center">
                                      <div class="">
                                        <i class="mdi mdi-cloud-download" style="font-size: 24px; color: #B50027;"></i>
                                      </div>
                                    </a> -->
                                  </div>
                              </template>
                            </b-table>
                          </div>
                          <div class="row">
                            <b-col>
                              <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
                            </b-col>
                            <div class="col">
                              <div
                                  class="dataTables_paginate paging_simple_numbers float-end">
                                <ul class="pagination pagination-rounded mb-0">
                                  <!-- pagination -->
                                  <b-pagination
                                      v-model="currentPage"
                                      :total-rows="totalRows"
                                      :per-page="perPage"
                                  ></b-pagination>
                                </ul>
                              </div>
                            </div>
                          </div>

                        </div>

                        <div class="col-lg-3">
                          <div class="category">
                            <div class="cate-title">
                              Danh Mục
                            </div>
                            <div v-for ="(item,index) in listDanhMuc" :key="index"  class="cate-list">
                              <ul style="cursor: pointer">
                                <li>
                                  <i class="mdi mdi-chevron-double-right"></i>
                                  <a :href= "item.link">{{item.label}}</a>
                                </li>
                              </ul>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

              </div>
            </div>
          </div>
        </div>
    </section>
  </Layout>
</template>
<style>

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
.color-primary {
  /*color: #28883B;*/
  color: #cc3333
}

.bg-primary {
  /*background-color: #28883B !important;*/
  background-color:#cc3333 !important;
}

table thead tr th{
  background-color: #efc62c !important;
  border: 1px solid #747070 !important;
}
table tbody tr td{
  border: 1px solid #747070 !important;
}
.td-stt{
  text-align: center;
  width: 5%;
}
.td-skh{
  width: 20%;
}
.td-nk{
  width: 10%;
  text-align: center;
}
.td-download{
  text-align: center;
}

.active > .page-link {
  background-color: #EFC62C !important;
}
.search-box{
    display: flex !important;
    justify-content: center;
    align-items: center;
}

</style>
