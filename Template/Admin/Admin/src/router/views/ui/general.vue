<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

/**
 * General-ui component
 */
export default {
  page: {
    title: "General UI",
    meta: [
      {
        name: "description",
        content: appConfig.description,
      },
    ],
  },
  components: {
    Layout,
    PageHeader,
  },
  data() {
    return {
      title: "General",
      items: [
        {
          text: "UI Elements",
          href: "/",
        },
        {
          text: "General",
          active: true,
        },
      ],
      defaultvalue: 3,
      currentpage: 3,
    };
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="col-lg-6">
                <div>
                  <h4 class="card-title">Badges</h4>
                  <p class="card-title-desc">
                    Add any of the below mentioned modifier classes to change
                    the appearance of a badge.
                  </p>
                  <div>
                    <span class="badge bg-primary">Primary</span>
                    <span class="badge bg-success ms-1">Success</span>
                    <span class="badge bg-info ms-1">Info</span>
                    <span class="badge bg-warning ms-1">Warning</span>
                    <span class="badge bg-danger ms-1">Danger</span>
                    <span class="badge bg-dark ms-1">Dark</span>
                  </div>
                </div>
              </div>
              <div class="col-lg-6">
                <div class="mt-4 mt-lg-0">
                  <h4 class="card-title">Pill Badges</h4>
                  <p class="card-title-desc">
                    Use the
                    <code>.badge-pill</code> modifier class to make badges more
                    rounded (with a larger
                    <code>border-radius</code>
                    and additional horizontal
                    <code>padding</code>). Useful if you miss the badges from
                    v3.
                  </p>

                  <div>
                    <span class="badge rounded-pill bg-primary">Primary</span>
                    <span class="badge rounded-pill bg-success ms-1"
                      >Success</span
                    >
                    <span class="badge rounded-pill bg-info ms-1">Info</span>
                    <span class="badge rounded-pill bg-warning ms-1"
                      >Warning</span
                    >
                    <span class="badge rounded-pill bg-danger ms-1"
                      >Danger</span
                    >
                    <span class="badge rounded-pill bg-dark ms-1">Dark</span>
                  </div>
                </div>
              </div>
            </div>
            <!-- end row -->
          </div>
        </div>
        <!-- end card -->
      </div>
    </div>
    <!-- end row -->

    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Popovers</h4>
            <p class="card-title-desc">
              Add small overlay content, like those found in iOS, to any element
              for housing secondary information.
            </p>

            <div class="button-items">
              <b-button
                v-b-popover.top="
                  'Vivamus sagittis lacus vel augue laoreet rutrum faucibus.'
                "
                variant="secondary"
                >Popover on top</b-button
              >

              <b-button
                v-b-popover.bottom="
                  'Vivamus sagittis lacus vel augue laoreet rutrum faucibus.'
                "
                variant="secondary"
                >Popover on bottom</b-button
              >
              <b-button
                v-b-popover.right="
                  'Vivamus sagittis lacus vel augue laoreet rutrum faucibus.'
                "
                variant="secondary"
                >Popover on right</b-button
              >
              <b-button
                v-b-popover.left="
                  'Vivamus sagittis lacus vel augue laoreet rutrum faucibus.'
                "
                variant="secondary"
                >Popover on left</b-button
              >
            </div>
          </div>
        </div>
      </div>

      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Tooltips</h4>
            <p class="card-title-desc">
              Hover over the links below to see tooltips:
            </p>

            <div class="button-items">
              <b-button id="tooltip-top" variant="primary"
                >Tooltip on top</b-button
              >

              <b-tooltip
                target="tooltip-top"
                placement="top"
                title="Tooltip on top"
              >
              </b-tooltip>

              <b-button id="tooltip-bottom" variant="primary"
                >Tooltip on bottom</b-button
              >
              <b-tooltip
                target="tooltip-bottom"
                placement="bottom"
                title="Tooltip on bottom"
              >
              </b-tooltip>

              <b-button id="tooltip-left" variant="primary"
                >Tooltip on left</b-button
              >
              <b-tooltip
                target="tooltip-left"
                placement="left"
                title="Tooltip on left"
              >
              </b-tooltip>

              <b-button id="tooltip-right" variant="primary"
                >Tooltip on right</b-button
              >
              <b-tooltip
                target="tooltip-right"
                placement="right"
                title="Tooltip on right"
              >
              </b-tooltip>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Pagination</h4>

            <div class="row">
              <div class="col-lg-6">
                <h5 class="font-size-14">Default Example</h5>
                <p class="card-title-desc">
                  Pagination links indicate a series of related content exists
                  across multiple pages.
                </p>
                <b-pagination
                  v-model="defaultvalue"
                  :total-rows="50"
                  :per-page="10"
                  aria-controls="my-table"
                ></b-pagination>
              </div>

              <div class="col-lg-6">
                <div class="mt-4">
                  <h5 class="card-title">Alignment</h5>
                  <p class="card-title-desc">
                    Change the alignment of pagination components with flexbox
                    utilities.
                  </p>

                  <b-pagination
                    :total-rows="50"
                    prev-text="Previous"
                    next-text="Next"
                    align="center"
                  ></b-pagination>

                  <b-pagination
                    :total-rows="50"
                    prev-text="Previous"
                    next-text="Next"
                    align="right"
                  ></b-pagination>
                </div>
              </div>
            </div>
            <!-- end row -->

            <div class="row">
              <div class="col-lg-6">
                <div class="mt-4">
                  <h5 class="font-size-14">Sizing</h5>
                  <p class="card-title-desc">
                    Fancy larger or smaller pagination? Add
                    <code>.pagination-lg</code> or
                    <code>.pagination-sm</code> for additional sizes.
                  </p>
                  <b-pagination
                    size="lg"
                    :total-rows="30"
                    :per-page="10"
                    aria-controls="my-table"
                  ></b-pagination>

                  <b-pagination
                    size="sm"
                    :total-rows="30"
                    :per-page="10"
                    aria-controls="my-table"
                  ></b-pagination>
                </div>
              </div>
            </div>
            <!-- end row -->
          </div>
        </div>
        <!-- end card -->
      </div>
    </div>
    <!-- end row -->

    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Border spinner</h4>
            <p class="card-title-desc">
              Use the border spinners for a lightweight loading indicator.
            </p>
            <div>
              <b-spinner
                class="m-2"
                variant="primary"
                role="status"
              ></b-spinner>
              <b-spinner
                class="m-2"
                variant="secondary"
                role="status"
              ></b-spinner>
              <b-spinner
                class="m-2"
                variant="success"
                role="status"
              ></b-spinner>
              <b-spinner class="m-2" variant="info" role="status"></b-spinner>
              <b-spinner
                class="m-2"
                variant="warning"
                role="status"
              ></b-spinner>
              <b-spinner class="m-2" variant="danger" role="status"></b-spinner>
              <b-spinner class="m-2" variant="dark" role="status"></b-spinner>
            </div>
          </div>
        </div>
      </div>
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Growing spinner</h4>
            <p class="card-title-desc">
              If you don’t fancy a border spinner, switch to the grow spinner.
              While it doesn’t technically spin, it does repeatedly grow!
            </p>
            <div>
              <b-spinner
                type="grow"
                class="m-2"
                variant="primary"
                role="status"
              ></b-spinner>
              <b-spinner
                type="grow"
                class="m-2"
                variant="secondary"
                role="status"
              ></b-spinner>
              <b-spinner
                type="grow"
                class="m-2"
                variant="success"
                role="status"
              ></b-spinner>
              <b-spinner
                type="grow"
                class="m-2"
                variant="info"
                role="status"
              ></b-spinner>
              <b-spinner
                type="grow"
                class="m-2"
                variant="warning"
                role="status"
              ></b-spinner>
              <b-spinner
                type="grow"
                class="m-2"
                variant="danger"
                role="status"
              ></b-spinner>
              <b-spinner
                type="grow"
                class="m-2"
                variant="dark"
                role="status"
              ></b-spinner>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->
  </Layout>
</template>
